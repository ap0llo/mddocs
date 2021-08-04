using System;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Loaders;
using Grynwald.MdDocs.ApiReference.Model;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Loaders
{
    /// <summary>
    /// Tests for <see cref="ApiReferenceBuilder"/>
    /// </summary>
    public class ApiReferenceBuilderTest
    {
        [Fact]
        public void Collections_are_initially_empty()
        {
            // ARRANGE
            var sut = new ApiReferenceBuilder();

            // ACT 

            // ASSERT
            Assert.Empty(sut.Assemblies);
            Assert.Empty(sut.Namespaces);
            Assert.Empty(sut.Types);
        }

        public class AddAssembly
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData(" ")]
            [InlineData("\t")]
            public void Throws_ArgumentException_when_assembly_name_is_null_or_whitespace(string assemblyName)
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT
                var ex = Record.Exception(() => sut.AddAssembly(assemblyName: assemblyName, assemblyVersion: "1.0.0"));

                // ASSERT
                var argumentException = Assert.IsType<ArgumentException>(ex);
                Assert.Equal("assemblyName", argumentException.ParamName);
            }

            [Fact]
            public void Returns_new_assembly()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT 
                var addedAssembly1 = sut.AddAssembly("SomeAssemblyName1", assemblyVersion: null);
                var addedAssembly2 = sut.AddAssembly("SomeAssemblyName2", assemblyVersion: "1.0.0");

                // ASSERT
                Assert.NotNull(addedAssembly1);
                Assert.Equal("SomeAssemblyName1", addedAssembly1.Name);
                Assert.Null(addedAssembly1.Version);

                Assert.NotNull(addedAssembly2);
                Assert.Equal("SomeAssemblyName2", addedAssembly2.Name);
                Assert.Equal("1.0.0", addedAssembly2.Version);

                Assert.Collection(
                    sut.Assemblies.OrderBy(x => x.Name),
                    assembly => Assert.Same(addedAssembly1, assembly),
                    assembly => Assert.Same(addedAssembly2, assembly)
                );
            }

            [Fact]
            public void Throws_DuplicateItemException_if_assembly_already_exists()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT 
                _ = sut.AddAssembly(assemblyName: "SomeAssemblyName", assemblyVersion: "1.0.0");
                var ex = Record.Exception(() => sut.AddAssembly(assemblyName: "someassemblyname", assemblyVersion: "1.0.0"));

                // ASSERT
                Assert.IsType<DuplicateItemException>(ex);
                Assert.Contains("Assembly 'someassemblyname' already exists", ex.Message);
            }
        }


        public class GetAssembly
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData(" ")]
            [InlineData("\t")]
            public void Throws_ArgumentException_when_assembly_name_is_null_or_whitespace(string assemblyName)
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT
                var ex = Record.Exception(() => sut.GetAssembly(assemblyName: assemblyName));

                // ASSERT
                var argumentException = Assert.IsType<ArgumentException>(ex);
                Assert.Equal("assemblyName", argumentException.ParamName);
            }

            [Fact]
            public void Throws_ItemNotFoundException_if_assembly_does_not_exist()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT 
                var ex = Record.Exception(() => sut.GetAssembly("AssemblyName"));

                // ASSERT
                Assert.IsType<ItemNotFoundException>(ex);
                Assert.Contains("Assembly 'AssemblyName' was not found", ex.Message);
            }

            [Fact]
            public void Returns_existing_assembly_if_it_already_exists()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();
                _ = sut.AddAssembly("SomeAssemblyName", "1.2.3");

                // ACT 
                var assembly1 = sut.GetAssembly(assemblyName: "SomeAssemblyName");
                var assembly2 = sut.GetAssembly(assemblyName: "someassemblyname");

                // ASSERT
                Assert.Same(assembly1, assembly2);
                Assert.Equal("1.2.3", assembly1.Version);
            }
        }


        public class AddNamespace
        {
            [Fact]
            public void AddNamespace_returns_global_namespace_when_namespace_name_is_empty()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT 
                var addedNamesapce = sut.AddNamespace("");

                // ASSERT
                Assert.Same(sut.GlobalNamespace, addedNamesapce);
                Assert.Collection(
                    sut.Namespaces.OrderBy(x => x.Name),
                    ns => Assert.Same(sut.GlobalNamespace, ns)
                );
            }

            [Fact]
            public void AddNamespace_implicitly_adds_the_global_namespace()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT 
                var namespace1 = sut.AddNamespace("Namespace1");

                // ASSERT
                Assert.Collection(
                    sut.Namespaces.OrderBy(x => x.Name),
                    ns => Assert.Same(sut.GlobalNamespace, ns),
                    ns => Assert.Same(namespace1, ns)
                );
            }

            [Fact]
            public void AddNamespace_implicitly_adds_all_parent_namespaces()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT 
                var addedNamespace = sut.AddNamespace("Namespace1.Namespace2.Namespace3");

                // ASSERT
                Assert.Collection(
                    sut.Namespaces.OrderBy(x => x.Name),
                    ns => Assert.Same(sut.GlobalNamespace, ns),
                    ns => Assert.Equal("Namespace1", ns.Name),
                    ns => Assert.Equal("Namespace1.Namespace2", ns.Name),
                    ns => Assert.Same(addedNamespace, ns)
                );
            }

            [Fact]
            public void AddNamespace_throws_DuplicateItemException_when_namespace_already_exists()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT 
                _ = sut.AddNamespace("Namespace1.Namespace2.Namespace3");
                var ex = Record.Exception(() => sut.AddNamespace("Namespace1.Namespace2.Namespace3"));

                // ASSERT
                Assert.IsType<DuplicateItemException>(ex);
                Assert.Contains("Namespace 'Namespace1.Namespace2.Namespace3' already exists", ex.Message);
            }

            [Fact]
            public void AddNamespace_reuses_parent_namespaces_if_they_were_already_added()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT 
                var namespace1 = sut.AddNamespace("Namespace1");
                var namespace2 = sut.AddNamespace("Namespace1.Namespace2");
                var namespace3 = sut.AddNamespace("Namespace1.Namespace2.Namespace3");

                // ASSERT
                Assert.Collection(
                    sut.Namespaces.OrderBy(x => x.Name),
                    // Global Namespace
                    ns =>
                    {
                        Assert.Same(sut.GlobalNamespace, ns);
                    },
                    // Namespace1
                    ns =>
                    {
                        Assert.Same(namespace1, ns);
                        Assert.Same(sut.GlobalNamespace, ns.ParentNamespace);
                    },
                    // Namespace1.Namespace2
                    ns =>
                    {
                        Assert.Same(namespace2, ns);
                        Assert.Same(namespace1, ns.ParentNamespace);
                    },
                    // Namespace1.Namespace2.Namespace3
                    ns =>
                    {
                        Assert.Same(namespace3, ns);
                        Assert.Same(namespace2, ns.ParentNamespace);
                    }
                );
            }

            [Fact]
            public void AddNamespace_adds_the_namespace_to_the_parent_namespace()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT 
                var namespace1 = sut.AddNamespace("Namespace1");
                var namespace2 = sut.AddNamespace("Namespace1.Namespace2");
                var namespace3 = sut.AddNamespace("Namespace1.Namespace2.Namespace3");
                var namespace4 = sut.AddNamespace("Namespace4");

                // ASSERT
                Assert.Collection(
                    sut.Namespaces.OrderBy(x => x.Name),
                    // Global Namespace
                    ns =>
                    {
                        Assert.Same(sut.GlobalNamespace, ns);
                        Assert.Collection(
                            ns.Namespaces.OrderBy(x => x.Name),
                            innerNamespace => Assert.Same(namespace1, innerNamespace),
                            innerNamespace => Assert.Same(namespace4, innerNamespace)
                        );
                    },
                    // Namespace1
                    ns =>
                    {
                        Assert.Same(namespace1, ns);
                        Assert.Collection(
                            ns.Namespaces.OrderBy(x => x.Name),
                            innerNamespace => Assert.Same(namespace2, innerNamespace)
                        );
                    },
                    // Namespace1.Namespace2
                    ns =>
                    {
                        Assert.Same(namespace2, ns);
                        Assert.Collection(
                            ns.Namespaces.OrderBy(x => x.Name),
                            innerNamespace => Assert.Same(namespace3, innerNamespace)
                        );

                    },
                    // Namespace1.Namespace2.Namespace3
                    ns =>
                    {
                        Assert.Same(namespace3, ns);
                        Assert.Empty(ns.Namespaces);
                    },
                    // Namespace4
                    ns =>
                    {
                        Assert.Same(namespace4, ns);
                        Assert.Empty(ns.Namespaces);
                    }
                );
            }

            [Theory]
            [InlineData(null)]
            // empty string is omitted here because a empty string is a valid namespace name (= global namespace)
            [InlineData(" ")]
            [InlineData("\t")]
            public void AddNamespace_throws_ArgumentException_when_namespace_name_is_null_or_whitespace(string namespaceName)
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT
                var ex = Record.Exception(() => sut.AddNamespace(namespaceName: namespaceName));

                // ASSERT
                var argumentException = Assert.IsType<ArgumentException>(ex);
                Assert.Equal("namespaceName", argumentException.ParamName);
            }

        }

        public class GetOrAddNamespace
        {
            [Fact]
            public void GetOrAddNamespace_returns_global_namespace_when_namespace_name_is_empty()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT 
                var addedNamesapce = sut.GetOrAddNamespace("");

                // ASSERT
                Assert.Same(sut.GlobalNamespace, addedNamesapce);
                Assert.Collection(
                    sut.Namespaces.OrderBy(x => x.Name),
                    ns => Assert.Same(sut.GlobalNamespace, ns)
                );
            }

            [Fact]
            public void GetOrAddNamespace_implicitly_adds_the_global_namespace()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT 
                var namespace1 = sut.GetOrAddNamespace("Namespace1");

                // ASSERT
                Assert.Collection(
                    sut.Namespaces.OrderBy(x => x.Name),
                    ns => Assert.Same(sut.GlobalNamespace, ns),
                    ns => Assert.Same(namespace1, ns)
                );
            }

            [Fact]
            public void GetOrAddNamespace_implicitly_adds_all_parent_namespaces()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT 
                var addedNamespace = sut.GetOrAddNamespace("Namespace1.Namespace2.Namespace3");

                // ASSERT
                Assert.Collection(
                    sut.Namespaces.OrderBy(x => x.Name),
                    ns => Assert.Same(sut.GlobalNamespace, ns),
                    ns => Assert.Equal("Namespace1", ns.Name),
                    ns => Assert.Equal("Namespace1.Namespace2", ns.Name),
                    ns => Assert.Same(addedNamespace, ns)
                );
            }

            [Fact]
            public void GetOrAddNamespace_returns_existing_instance_when_adding_a_namespace_that_already_exists()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT 
                var addedNamespace1 = sut.GetOrAddNamespace("Namespace1.Namespace2.Namespace3");
                var addedNamespace2 = sut.GetOrAddNamespace("Namespace1.Namespace2.Namespace3");

                // ASSERT
                Assert.Same(addedNamespace1, addedNamespace2);
            }

            [Fact]
            public void GetOrAddNamespace_reuses_parent_namespaces_if_they_were_already_added()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT 
                var namespace1 = sut.GetOrAddNamespace("Namespace1");
                var namespace2 = sut.GetOrAddNamespace("Namespace1.Namespace2");
                var namespace3 = sut.GetOrAddNamespace("Namespace1.Namespace2.Namespace3");

                // ASSERT
                Assert.Collection(
                    sut.Namespaces.OrderBy(x => x.Name),
                    // Global Namespace
                    ns =>
                    {
                        Assert.Same(sut.GlobalNamespace, ns);
                    },
                    // Namespace1
                    ns =>
                    {
                        Assert.Same(namespace1, ns);
                        Assert.Same(sut.GlobalNamespace, ns.ParentNamespace);
                    },
                    // Namespace1.Namespace2
                    ns =>
                    {
                        Assert.Same(namespace2, ns);
                        Assert.Same(namespace1, ns.ParentNamespace);
                    },
                    // Namespace1.Namespace2.Namespace3
                    ns =>
                    {
                        Assert.Same(namespace3, ns);
                        Assert.Same(namespace2, ns.ParentNamespace);
                    }
                );
            }

            [Fact]
            public void GetOrAddNamespace_adds_the_namespace_to_the_parent_namespace()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT 
                var namespace1 = sut.GetOrAddNamespace("Namespace1");
                var namespace2 = sut.GetOrAddNamespace("Namespace1.Namespace2");
                var namespace3 = sut.GetOrAddNamespace("Namespace1.Namespace2.Namespace3");
                var namespace4 = sut.GetOrAddNamespace("Namespace4");

                // ASSERT
                Assert.Collection(
                    sut.Namespaces.OrderBy(x => x.Name),
                    // Global Namespace
                    ns =>
                    {
                        Assert.Same(sut.GlobalNamespace, ns);
                        Assert.Collection(
                            ns.Namespaces.OrderBy(x => x.Name),
                            innerNamespace => Assert.Same(namespace1, innerNamespace),
                            innerNamespace => Assert.Same(namespace4, innerNamespace)
                        );
                    },
                    // Namespace1
                    ns =>
                    {
                        Assert.Same(namespace1, ns);
                        Assert.Collection(
                            ns.Namespaces.OrderBy(x => x.Name),
                            innerNamespace => Assert.Same(namespace2, innerNamespace)
                        );
                    },
                    // Namespace1.Namespace2
                    ns =>
                    {
                        Assert.Same(namespace2, ns);
                        Assert.Collection(
                            ns.Namespaces.OrderBy(x => x.Name),
                            innerNamespace => Assert.Same(namespace3, innerNamespace)
                        );

                    },
                    // Namespace1.Namespace2.Namespace3
                    ns =>
                    {
                        Assert.Same(namespace3, ns);
                        Assert.Empty(ns.Namespaces);
                    },
                    // Namespace4
                    ns =>
                    {
                        Assert.Same(namespace4, ns);
                        Assert.Empty(ns.Namespaces);
                    }
                );
            }

            [Theory]
            [InlineData(null)]
            // empty string is omitted here because a empty string is a valid namespace name (= global namespace)
            [InlineData(" ")]
            [InlineData("\t")]
            public void GetOrAddNamespace_throws_ArgumentException_when_namespace_name_is_null_or_whitespace(string namespaceName)
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT
                var ex = Record.Exception(() => sut.GetOrAddNamespace(namespaceName: namespaceName));

                // ASSERT
                var argumentException = Assert.IsType<ArgumentException>(ex);
                Assert.Equal("namespaceName", argumentException.ParamName);
            }

        }

        public class AddType
        {
            [Fact]
            public void AddType_returns_expected_type_and_adds_it_to_the_list_of_types()
            {
                // ARRANGE
                var typeId = new SimpleTypeId("Namespace1", "Class1");

                var sut = new ApiReferenceBuilder();
                var assembly = sut.AddAssembly("Assembly1", null);
                var @namespace = sut.GetOrAddNamespace("Namespace1");

                // ACT
                var type = sut.AddType("Assembly1", typeId);

                // ASSERT
                Assert.NotNull(type);
                Assert.Same(assembly, type.Assembly);
                Assert.Same(@namespace, type.Namespace);
                Assert.Equal(typeId, type.TypeId);
                Assert.Equal("Class1", type.DisplayName);

                Assert.Collection(
                    sut.Types,
                    t => Assert.Same(type, t)
                );
            }

            [Fact]
            public void AddType_throws_DuplicateItemException_if_type_already_exists()
            {
                // ARRANGE
                var typeId = new SimpleTypeId("Namespace1", "Class1");

                var sut = new ApiReferenceBuilder();
                _ = sut.AddType("Assembly1", typeId);

                // ACT
                var ex = Record.Exception(() => sut.AddType("Assembly2", typeId));

                // ASSERT
                Assert.IsType<DuplicateItemException>(ex);
                Assert.Contains("Type 'Namespace1.Class1' already exists", ex.Message);
            }
        }

        public class GetType_
        {
            [Fact]
            public void GetType_throws_ItemNotFoundException_if_type_does_not_exist()
            {
                // ARRANGE                
                var typeId = new SimpleTypeId("Namespace1", "Class1");

                var sut = new ApiReferenceBuilder();

                // ACT
                var ex = Record.Exception(() => sut.GetType(typeId));

                // ASSERT
                Assert.IsType<ItemNotFoundException>(ex);
                Assert.Contains("Type 'Namespace1.Class1' was not found", ex.Message);
            }

            [Fact]
            public void GetType_returns_expected_type()
            {
                // ARRANGE
                var typeId = new SimpleTypeId("Namespace1", "Class1");

                var sut = new ApiReferenceBuilder();
                var addedType = sut.AddType("Assembly1", typeId);

                // ACT
                var retrievedType = sut.GetType(typeId);

                // ASSERT
                Assert.Same(addedType, retrievedType);
            }
        }


        public class Build
        {
            [Fact]
            public void Returns_empty_set_if_no_assemblies_or_types_were_added()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT                
                var assemblySet = sut.Build();

                // ASSERT
                Assert.NotNull(assemblySet);
                Assert.Empty(assemblySet.Assemblies);
                Assert.Empty(assemblySet.Namespaces);
                Assert.Empty(assemblySet.Types);
            }

            [Fact]
            public void Returns_expected_assembly_set()
            {
                // ARRANGE
                var sut = new ApiReferenceBuilder();

                // ACT
                _ = sut.AddAssembly("Assembly1", "1.2.3");
                _ = sut.AddAssembly("Assembly2", "4.5.6");
                _ = sut.GetOrAddNamespace("Namespace1");
                _ = sut.GetOrAddNamespace("Namespace2");
                _ = sut.GetOrAddNamespace("Namespace3.Namespace4");

                var assemblySet = sut.Build();

                // ASSERT
                Assert.NotNull(assemblySet);
                Assert.Collection(
                    assemblySet.Assemblies.OrderBy(x => x.Name),
                    assembly => Assert.Equal("Assembly1", assembly.Name),
                    assembly => Assert.Equal("Assembly2", assembly.Name)
                );
                Assert.Collection(
                    assemblySet.Namespaces.OrderBy(x => x.Name),
                    ns => Assert.Equal(NamespaceId.GlobalNamespace, ns.NamespaceId),
                    ns => Assert.Equal("Namespace1", ns.Name),
                    ns => Assert.Equal("Namespace2", ns.Name),
                    ns => Assert.Equal("Namespace3", ns.Name),
                    ns => Assert.Equal("Namespace3.Namespace4", ns.Name)
                );
                Assert.Empty(assemblySet.Types);
            }
        }

    }
}
