using System;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Loaders;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.TestHelpers;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Test.Loaders
{
    /// <summary>
    /// Tests for <see cref="MonoCecilDocumentationLoader"/>
    /// </summary>
    public class MonoCecilDocumentationLoaderTest : DynamicCompilationTestBase
    {
        private readonly ILogger m_Logger;


        public MonoCecilDocumentationLoaderTest(ITestOutputHelper testOutputHelper)
        {
            m_Logger = new XunitLogger(testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper)));
        }


        [Theory]
        [InlineData("Assembly1", "Assembly1")]
        [InlineData("Assembly1", "assembly1")]
        [InlineData("ASSEMBLY1", "Assembly1")]
        public void Load_throws_DuplicateItemException_if_set_contains_multiple_assemblies_with_the_same_name(string assemblyName1, string assemblyName2)
        {
            // ARRANGE
            var assembly1 = Compile("", assemblyName1);
            var assembly2 = Compile("", assemblyName2);

            var sut = new MonoCecilDocumentationLoader(m_Logger);

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

            var sut = new MonoCecilDocumentationLoader(m_Logger);

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

            var sut = new MonoCecilDocumentationLoader(m_Logger);

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

            var sut = new MonoCecilDocumentationLoader(m_Logger);

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

            var sut = new MonoCecilDocumentationLoader(m_Logger);

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

            var sut = new MonoCecilDocumentationLoader(m_Logger);


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

            var sut = new MonoCecilDocumentationLoader(m_Logger);

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
                    Assert.Equal(8, assembly.Types.Count);
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
                    Assert.Equal(8, ns.Types.Count);
                    Assert.All(assemblySet.Types,
                        type => Assert.Contains(ns.Types, x => x == type)
                    );
                }
            );

            var ordered = assemblySet.Types.Select(x => x.DisplayName).OrderBy(x => x).ToList();

            Assert.Collection(
                assemblySet.Types.OrderBy(x => x.DisplayName),
                // Class1
                type =>
                {
                    Assert.Equal(class1, type.TypeId);
                    Assert.Equal(TypeKind.Class, type.Kind);
                },
                // Class2
                type =>
                {
                    Assert.Equal(class2, type.TypeId);
                    Assert.Equal(TypeKind.Class, type.Kind);
                },
                // NestedClass1
                type =>
                {
                    Assert.Equal(nestedClass1, type.TypeId);
                    Assert.Equal(TypeKind.Class, type.Kind);
                },
                // NestedClass2
                type =>
                {
                    Assert.Equal(nestedClass2, type.TypeId);
                    Assert.Equal(TypeKind.Class, type.Kind);
                },
                // NestedClass3
                type =>
                {
                    Assert.Equal(nestedClass3, type.TypeId);
                    Assert.Equal(TypeKind.Class, type.Kind);
                },
                // Enum1
                type =>
                {
                    Assert.Equal(enum1, type.TypeId);
                    Assert.Equal(TypeKind.Enum, type.Kind);
                },
                // Interface1
                type =>
                {
                    Assert.Equal(interface1, type.TypeId);
                    Assert.Equal(TypeKind.Interface, type.Kind);
                },
                // Struct1
                type =>
                {
                    Assert.Equal(struct1, type.TypeId);
                    Assert.Equal(TypeKind.Struct, type.Kind);
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
            var sut = new MonoCecilDocumentationLoader(m_Logger);

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

            var sut = new MonoCecilDocumentationLoader(m_Logger);

            // ACT
            var assemblySet = sut.Load(new[] { assembly });

            // ASSERT          
            Assert.DoesNotContain(assemblySet.Types, t => t.TypeId.Name == "NestedClass1");
            Assert.DoesNotContain(assemblySet.Types, t => t.TypeId.Name == "NestedClass2");
        }

        //TODO 2021-08-04: Load returns assemblies with expected names, version and type (also port over appropriate tests from AssemblyDocumentationTest)
        //TODO 2021-08-04: Loader loads expected types
        //TODO 2021-08-04: Loader adds types to namespaces

        [Theory]
        [InlineData("public class Class1", TypeKind.Class)]
        [InlineData("public struct Struct1", TypeKind.Struct)]
        [InlineData("public interface Interface1", TypeKind.Interface)]
        [InlineData("public enum Enum1", TypeKind.Enum)]
        public void Load_sets_the_Kind_property_on_types(string definition, TypeKind expectedKind)
        {
            // ARRANGE
            var cs = $@"
                using System;

                {definition}
                {{ }}
            ";

            using var assembly = Compile(cs);

            var sut = new MonoCecilDocumentationLoader(m_Logger);

            // ACT
            var assemblySet = sut.Load(new[] { assembly });

            // ASSERT

            Assert.Collection(assemblySet.Types, type => Assert.Equal(expectedKind, type.Kind));
        }

        [Fact]
        public void Load_reads_a_types_fields()
        {
            // ARRANGE 
            var cs = @"
	                using System;

	                public class Class1
	                {
                        public int Field1;
                        private bool Field2;
                    }
                ";

            using var assembly = Compile(cs);

            var sut = new MonoCecilDocumentationLoader(m_Logger);

            // ACT
            var assemblySet = sut.Load(new[] { assembly });

            // ASSERT
            Assert.Collection(
                assemblySet.Types,
                type =>
                {
                    Assert.Collection(
                        type.Fields,
                        field =>
                        {
                            Assert.Equal("Field1", field.Name);
                            Assert.Equal(new SimpleTypeId("System", "Int32"), field.Type);
                            Assert.Equal(new FieldId(type.TypeId, "Field1"), field.MemberId);
                            Assert.Same(type, field.DeclaringType);
                        });
                });
        }

        [Fact]
        public void Load_adds_expected_fields_for_enums()
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

            var sut = new MonoCecilDocumentationLoader(m_Logger);

            // ACT
            var assemblySet = sut.Load(new[] { assembly });

            // ASSERT
            Assert.Collection(
                assemblySet.Types,
                type =>
                {
                    Assert.Collection(
                        type.Fields.OrderBy(x => x.Name),
                        field =>
                        {
                            Assert.Equal("Value1", field.Name);
                            Assert.Equal(new SimpleTypeId("", "Enum1"), field.Type);
                            Assert.Equal(new FieldId(type.TypeId, "Value1"), field.MemberId);
                            Assert.Same(type, field.DeclaringType);
                        },
                        field =>
                        {
                            Assert.Equal("Value2", field.Name);
                            Assert.Equal(new SimpleTypeId("", "Enum1"), field.Type);
                            Assert.Equal(new FieldId(type.TypeId, "Value2"), field.MemberId);
                            Assert.Same(type, field.DeclaringType);
                        });
                });
        }


        [Fact]
        public void Load_reads_a_types_events()
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

	            public interface Interface1
	            {
                    event EventHandler Event1;

                    event EventHandler<EventArgs> Event2;

                    event EventHandler Event3;
                }
            ";

            using var assembly = Compile(cs);

            var sut = new MonoCecilDocumentationLoader(m_Logger);

            // ACT
            var assemblySet = sut.Load(new[] { assembly });

            // ASSERT
            Assert.All(
                assemblySet.Types,
                type =>
                {
                    Assert.Collection(
                        type.Events.OrderBy(x => x.Name),
                        ev =>
                        {
                            Assert.Equal("Event1", ev.Name);
                            Assert.Equal(new SimpleTypeId("System", "EventHandler"), ev.Type);
                            Assert.Equal(new ApiReference.Model.EventId(type.TypeId, "Event1"), ev.MemberId);
                            Assert.Same(type, ev.DeclaringType);
                        },
                        ev =>
                        {
                            Assert.Equal("Event2", ev.Name);
                            Assert.Equal(
                                new GenericTypeInstanceId("System", "EventHandler", new[] { new SimpleTypeId("System", "EventArgs") }),
                                ev.Type
                            );
                            Assert.Equal(new ApiReference.Model.EventId(type.TypeId, "Event2"), ev.MemberId);
                            Assert.Same(type, ev.DeclaringType);
                        },
                        ev =>
                        {
                            Assert.Equal("Event3", ev.Name);
                            Assert.Equal(new SimpleTypeId("System", "EventHandler"), ev.Type);
                            Assert.Equal(new ApiReference.Model.EventId(type.TypeId, "Event3"), ev.MemberId);
                            Assert.Same(type, ev.DeclaringType);
                        }
                    );

                });
        }

        [Fact]
        public void Loads_does_not_read_events_for_enums()
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
            var sut = new MonoCecilDocumentationLoader(m_Logger);

            // ACT
            var assemblySet = sut.Load(new[] { assembly });

            //ASSERT
            Assert.Collection(
                assemblySet.Types,
                type =>
                {
                    Assert.NotNull(type.Events);
                    Assert.Empty(type.Events);
                });
        }

        [Fact]
        public void Load_reads_a_type_s_properties_01()
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

            var sut = new MonoCecilDocumentationLoader(m_Logger);

            // ACT
            var assemblySet = sut.Load(new[] { assembly });

            // ASSERT
            Assert.Collection(
                assemblySet.Types,
                type =>
                {
                    Assert.Collection(
                        type.Properties.OrderBy(x => x.Name),
                        property =>
                        {
                            Assert.Equal("Property1", property.Name);
                            Assert.Equal(new SimpleTypeId("System", "Int32"), property.Type);
                            Assert.Equal(new PropertyId(type.TypeId, "Property1"), property.MemberId);
                            Assert.Same(type, property.DeclaringType);
                        },
                        property =>
                        {
                            Assert.Equal("Property2", property.Name);
                            Assert.Equal(new SimpleTypeId("System", "Int32"), property.Type);
                            Assert.Equal(new PropertyId(type.TypeId, "Property2"), property.MemberId);
                            Assert.Same(type, property.DeclaringType);
                        });
                });
        }

        [Fact]
        public void Load_reads_a_type_s_properties_02()
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

            var sut = new MonoCecilDocumentationLoader(m_Logger);

            // ACT
            var assemblySet = sut.Load(new[] { assembly });


            // ASSERT
            Assert.Collection(
                assemblySet.Types,
                type =>
                {
                    Assert.Collection(
                        type.Properties.OrderBy(x => x.Name),
                        property =>
                        {
                            Assert.Equal("Property1", property.Name);
                            Assert.Equal(new SimpleTypeId("System", "Int32"), property.Type);
                            Assert.Equal(new PropertyId(type.TypeId, "Property1"), property.MemberId);
                            Assert.Same(type, property.DeclaringType);
                        },
                        property =>
                        {
                            Assert.Equal("Property2", property.Name);
                            Assert.Equal(new SimpleTypeId("System", "Int32"), property.Type);
                            Assert.Equal(new PropertyId(type.TypeId, "Property2"), property.MemberId);
                            Assert.Same(type, property.DeclaringType);
                        });
                });
        }

        [Fact]
        public void Load_reads_a_type_s_properties_03()
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
            var sut = new MonoCecilDocumentationLoader(m_Logger);

            // ACT
            var assemblySet = sut.Load(new[] { assembly });


            // ASSERT
            Assert.Collection(
                assemblySet.Types,
                type =>
                {
                    Assert.Collection(
                        type.Properties.OrderBy(x => x.Name),
                        property =>
                        {
                            Assert.Equal("Property1", property.Name);
                            Assert.Equal(new SimpleTypeId("System", "Int32"), property.Type);
                            Assert.Equal(new PropertyId(type.TypeId, "Property1"), property.MemberId);
                            Assert.Same(type, property.DeclaringType);
                        },
                        property =>
                        {
                            Assert.Equal("Property2", property.Name);
                            Assert.Equal(new SimpleTypeId("System", "Int32"), property.Type);
                            Assert.Equal(new PropertyId(type.TypeId, "Property2"), property.MemberId);
                            Assert.Same(type, property.DeclaringType);
                        });
                });
        }

    }
}
