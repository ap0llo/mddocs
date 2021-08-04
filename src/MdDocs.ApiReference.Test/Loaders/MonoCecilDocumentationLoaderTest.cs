using System;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Loaders;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.TestHelpers;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Loaders
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
        public void Load_throws_DuplicateItemException_if_set_contains_multiple_assemblies_with_the_same_name(string assemblyName1, string assemblyName2)
        {
            // ARRANGE
            var assembly1 = Compile("", assemblyName1);
            var assembly2 = Compile("", assemblyName2);

            var sut = new MonoCecilDocumentationLoader();

            // ACT 
            var ex = Record.Exception(() => sut.Load(new[] { assembly1, assembly2 }));

            // ASSERT
            Assert.IsType<DuplicateItemException>(ex);
            Assert.Contains($"Assembly '{assemblyName1}' already exists", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Load_throws_DuplicateItemException_if_a_type_exists_in_multiple_assemblies()
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
            using var assembly1 = Compile(cs, "Assembly1");
            using var assembly2 = Compile(cs, "Assembly2");

            var sut = new MonoCecilDocumentationLoader();

            // ACT 
            var ex = Record.Exception(() => sut.Load(new[] { assembly1, assembly2 }));

            // ASSERT
            Assert.IsType<DuplicateItemException>(ex);
            Assert.Contains($"Type 'Namespace1.Class1' already exists", ex.Message);
        }

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

        [Fact]
        public void Load_returns_assembly_set_with_expected_types()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            { }

                public interface Interface1
                { }

                public enum Enum1
                {
                    Value1,
                    Value2
                }

                public struct Struct1
	            { }

                public class Class2
    	        {
                    public class NestedClass1
                    { }

                    public class NestedClass2
                    {
                        public class NestedClass3
                        { }
                    }
                }

            ";

            using var assemblyDefinition = Compile(cs, "Assembly1");

            var sut = new MonoCecilDocumentationLoader();

            var class1 = new SimpleTypeId(NamespaceId.GlobalNamespace, "Class1");
            var class2 = new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2");
            var enum1 = new SimpleTypeId(NamespaceId.GlobalNamespace, "Enum1");
            var interface1 = new SimpleTypeId(NamespaceId.GlobalNamespace, "Interface1");
            var nestedClass1 = new SimpleTypeId(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2"), "NestedClass1");
            var nestedClass2 = new SimpleTypeId(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2"), "NestedClass2");
            var nestedClass3 = new SimpleTypeId(new SimpleTypeId(new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2"), "NestedClass2"), "NestedClass3");
            var struct1 = new SimpleTypeId(NamespaceId.GlobalNamespace, "Struct1");

            // ACT
            var assemblySet = sut.Load(new[] { assemblyDefinition });

            // ASSERT
            Assert.Collection(
                assemblySet.Assemblies,
                assembly =>
                {
                    Assert.Equal("Assembly1", assembly.Name);
                    Assert.Equal(5, assembly.Types.Count);
                    Assert.All(assemblySet.Types,
                        type => Assert.Contains(assembly.Types, x => x == type)
                    );
                }
            );

            Assert.Collection(
                assemblySet.Namespaces,
                ns =>
                {
                    Assert.Equal(NamespaceId.GlobalNamespace, ns.NamespaceId);
                    Assert.Equal(5, ns.Types.Count);
                    Assert.All(assemblySet.Types,
                        type => Assert.Contains(ns.Types, x => x == type)
                    );
                }
            );

            Assert.Collection(
                assemblySet.Types.OrderBy(x => x.DisplayName),
                // Class1
                type =>
                {
                    Assert.Equal(class1, type.TypeId);
                },
                // Class2
                type =>
                {
                    Assert.Equal(class2, type.TypeId);
                },
                // Enum1
                type =>
                {
                    Assert.Equal(enum1, type.TypeId);
                },
                // Interface1
                type =>
                {
                    Assert.Equal(interface1, type.TypeId);
                },
                //// TODO 2021-08-04: NestedClass1
                //type => { },
                //// TODO 2021-08-04: NestedClass2
                //type => { },
                ////TODO 2021-08-04: NestedClass3
                //type => { },
                // Struct1
                type =>
                {
                    Assert.Equal(struct1, type.TypeId);
                }
            );
        }

        //TODO: types are added to namespaces and assemblies (not only the global namespace)

        [Fact]
        public void Load_ignores_internal_types()
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
            var sut = new MonoCecilDocumentationLoader();

            // ACT
            var assemblySet = sut.Load(new[] { assembly });

            // ASSERT          
            Assert.DoesNotContain(assemblySet.Types, t => t.TypeId.Name == "Class2");
        }

        [Fact]
        public void Load_ignores_non_public_nested_types()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    internal class NestedClass1
                    { }

                    class NestedClass2
                    { }
                }
            ";

            using var assembly = Compile(cs);

            var sut = new MonoCecilDocumentationLoader();

            // ACT
            var assemblySet = sut.Load(new[] { assembly });

            // ASSERT          
            Assert.DoesNotContain(assemblySet.Types, t => t.TypeId.Name == "NestedClass1");
            Assert.DoesNotContain(assemblySet.Types, t => t.TypeId.Name == "NestedClass2");
        }

        //TODO 2021-08-04: Load returns assemblies with expected names, version and type (also port over appropriate tests from AssemblyDocumentationTest)
        //TODO 2021-08-04: Loader loads expected types
        //TODO 2021-08-04: Loader adds types to namespaces
    }
}
