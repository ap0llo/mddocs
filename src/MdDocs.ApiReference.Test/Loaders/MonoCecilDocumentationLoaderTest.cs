using System;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Loaders;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.TestHelpers;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    /// <summary>
    /// Tests for <see cref="MonoCecilDocumentationLoader"/>
    /// </summary>
    public class MonoCecilDocumentationLoaderTest : DynamicCompilationTestBase
    {
        [Theory]
        [InlineData("Assembly1", "Assembly1")]
        [InlineData("Assembly1", "assembly1")]
        [InlineData("ASSEMBLY1", "Assembly1")]
        public void Load_throws_InvalidAssemblySetException_if_set_contains_multiple_assemblies_with_the_same_name(string assemblyName1, string assemblyName2)
        {
            // ARRANGE
            var assembly1 = Compile("", assemblyName1);
            var assembly2 = Compile("", assemblyName2);

            var sut = new MonoCecilDocumentationLoader();

            // ACT 
            var ex = Record.Exception(() => sut.Load(new[] { assembly1, assembly2 }));

            // ASSERT
            Assert.IsType<InvalidAssemblySetException>(ex);
            Assert.Contains($"multiple assemblies named {assemblyName1}", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        // TODO 2021-08-04
        //[Fact]
        //public void Load_throws_InvalidAssemblySetException_if_a_type_exists_in_multiple_assemblies()
        //{
        //    // ARRANGE
        //    var cs = @"
        //        namespace Namespace1
        //        {
        //            public class Class1
        //            {
        //            }
        //        }   
        //    ";
        //    using var assembly1 = Compile(cs, "Assembly1");
        //    using var assembly2 = Compile(cs, "Assembly2");

        //    var sut = new MonoCecilDocumentationLoader();

        //    // ACT 
        //    var ex = Record.Exception(() => sut.Load(new[] { assembly1, assembly2 }));

        //    // ASSERT
        //    Assert.IsType<InvalidAssemblySetException>(ex);
        //    Assert.Contains($"Type 'Namespace1.Class1' exists in multiple assemblies", ex.Message);
        //}

        [Fact]
        public void Load_does_not_throw_InvalidAssemblySetException_if_a_type_exists_in_multiple_assemblies_but_is_not_public()
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
            using var assembly1 = Compile(cs1, "Assembly1");
            using var assembly2 = Compile(cs2, "Assembly2");

            var sut = new MonoCecilDocumentationLoader();

            // ACT 
            var assemblySet = sut.Load(new[] { assembly1, assembly2 });

            // ASSERT
            Assert.Equal(2, assemblySet.Assemblies.Count);
        }

        [Fact]
        public void Load_returns_assembly_set_which_contains_expected_assemblies()
        {
            // ARRANGE
            var assembly1 = Compile("", "Assembly1");
            var assembly2 = Compile("", "Assembly2");

            var sut = new MonoCecilDocumentationLoader();

            // ACT
            var assemblySet = sut.Load(new[] { assembly1, assembly2 });

            // ASSERT
            Assert.Collection(
                assemblySet.Assemblies.OrderBy(x => x.Name),
                assembly => Assert.Equal("Assembly1", assembly.Name),
                assembly => Assert.Equal("Assembly2", assembly.Name)
            );
        }

        [Fact]
        public void Load_returns_assembly_set_which_contains_expected_namespaces()
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

            using var assembly1 = Compile(cs1, "Assembly1");
            using var assembly2 = Compile(cs2, "Assembly2");

            var sut = new MonoCecilDocumentationLoader();

            // ACT
            var assemblySet = sut.Load(new[] { assembly1, assembly2 });

            // ASSERT
            Assert.Collection(
                assemblySet.Namespaces.OrderBy(x => x.Name),
                ns => Assert.Equal(NamespaceId.GlobalNamespace, ns.NamespaceId),
                ns =>
                {
                    Assert.Equal(new NamespaceId("Namespace1"), ns.NamespaceId);
                    var childNamespace = Assert.Single(ns.Namespaces);
                    Assert.Same(assemblySet.Namespaces.Single(x => x.Name == "Namespace1.Namespace2"), childNamespace);
                },
                ns =>
                {
                    Assert.Equal(new NamespaceId("Namespace1.Namespace2"), ns.NamespaceId);
                    Assert.Equal(assemblySet.Namespaces.Single(x => x.Name == "Namespace1"), ns.ParentNamespace);
                },
                ns => Assert.Equal(new NamespaceId("Namespace3"), ns.NamespaceId),
                ns => Assert.Equal(new NamespaceId("Namespace4"), ns.NamespaceId)
            );

            var namespace1 = assemblySet.Namespaces.Single(x => x.Name == "Namespace1");
        }

        [Fact]
        public void Load_does_not_return_Namespaces_that_contain_only_internal_types()
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

            using var assembly1 = Compile(cs1, "Assembly1");
            using var assembly2 = Compile(cs2, "Assembly2");

            var sut = new MonoCecilDocumentationLoader();


            // ACT
            var assemblySet = sut.Load(new[] { assembly1, assembly2 });

            // ASSERT
            Assert.Collection(
                assemblySet.Namespaces.OrderBy(x => x.Name),
                ns => Assert.Equal(NamespaceId.GlobalNamespace, ns.NamespaceId),
                ns => Assert.Equal(new NamespaceId("Namespace1"), ns.NamespaceId)
            );

        }

        //TODO 2021-08-04: Load returns assemblies with expected names, version and type (also port over appropriate tests from AssemblyDocumentationTest)
        //TODO 2021-08-04: Loader loads expected types
        //TODO 2021-08-04: Loader adds types to namespaces
    }
}
