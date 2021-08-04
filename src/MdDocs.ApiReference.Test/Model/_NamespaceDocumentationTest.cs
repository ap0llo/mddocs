using System;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Loaders;
using Grynwald.MdDocs.ApiReference.Model;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    /// <summary>
    /// Tests for <see cref="_NamespaceDocumentation"/>
    /// </summary>
    public class _NamespaceDocumentationTest
    {
        public class Constructor
        {
            [Fact]
            public void ParentNamespace_may_be_null_if_namespaceId_is_global_namespace()
            {
                // ARRANGE

                // ACT 
                var sut = new _NamespaceDocumentation(parentNamespace: null, namespaceId: NamespaceId.GlobalNamespace);

                // ASSERT
                Assert.Equal(NamespaceId.GlobalNamespace, sut.NamespaceId);
                Assert.Equal("", sut.Name);
                Assert.Null(sut.ParentNamespace);
            }

            [Fact]
            public void ParentNamespace_must_not_be_null_if_namespaceId_is_not_global_namespace()
            {
                // ARRANGE

                // ACT 
                var ex = Record.Exception(() => new _NamespaceDocumentation(parentNamespace: null, namespaceId: new NamespaceId("Namespace1")));

                // ASSERT
                var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("parentNamespace", argumentNullException.ParamName);
            }

            [Fact]
            public void NamespaceId_must_not_be_null()
            {
                // ARRANGE

                // ACT 
                var ex = Record.Exception(() => new _NamespaceDocumentation(parentNamespace: null, namespaceId: null!));

                // ASSERT
                var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("namespaceId", argumentNullException.ParamName);
            }

            [Theory]
            [InlineData("")]
            [InlineData("Namespace1.Namespace2")]
            public void NamespaceId_must_start_with_id_of_parent_namespace_01(string namespaceName)
            {
                // ARRANGE                
                var globalNamespace = new _NamespaceDocumentation(null, NamespaceId.GlobalNamespace);

                // ACT 
                var ex = Record.Exception(() => new _NamespaceDocumentation(parentNamespace: globalNamespace, namespaceId: new NamespaceId(namespaceName)));

                // ASSERT
                Assert.IsType<InconsistentModelException>(ex);
                Assert.Contains(
                    $"Cannot initialize namespace '{(String.IsNullOrEmpty(namespaceName) ? "<GlobalNamespace>" : namespaceName)}' with parent namespace '<GlobalNamespace>'",
                    ex.Message
                );
            }

            [Fact]
            public void NamespaceId_must_start_with_id_of_parent_namespace_02()
            {
                // ARRANGE
                var globalNamespace = new _NamespaceDocumentation(null, NamespaceId.GlobalNamespace);
                var parentNamespace = new _NamespaceDocumentation(globalNamespace, new NamespaceId("Namespace1"));

                // ACT 
                var ex = Record.Exception(() => new _NamespaceDocumentation(parentNamespace: parentNamespace, namespaceId: new NamespaceId("Namespace2")));

                // ASSERT
                Assert.IsType<InconsistentModelException>(ex);
                Assert.Contains(
                     "Cannot initialize namespace 'Namespace2' with parent namespace 'Namespace1'",
                     ex.Message
                 );
            }

            [Fact]
            public void Namespaces_is_initially_empty()
            {
                // ARRANGE
                var globalNamespace = new _NamespaceDocumentation(null, NamespaceId.GlobalNamespace);

                // ACT
                var sut = new _NamespaceDocumentation(parentNamespace: globalNamespace, namespaceId: new NamespaceId("Namespace1"));

                // ASSERT
                Assert.NotNull(sut.Namespaces);
                Assert.Empty(sut.Namespaces);
            }
        }

        public class Add_Namespace
        {
            [Fact]
            public void Checks_arguments_for_null()
            {
                // ARRANGE
                var sut = new _NamespaceDocumentation(parentNamespace: null, namespaceId: NamespaceId.GlobalNamespace);

                // ACT 
                var ex = Record.Exception(() => sut.Add(@namespace: null!));

                // ASSERT
                var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("namespace", argumentNullException.ParamName);
            }

            [Fact]
            public void Throws_InconsistentModelException_if_namespace_being_added_is_not_a_child_of_the_current_namespace_01()
            {
                // ARRANGE
                var globalNamespace = new _NamespaceDocumentation(null, NamespaceId.GlobalNamespace);
                var sut = new _NamespaceDocumentation(parentNamespace: globalNamespace, namespaceId: new NamespaceId("Namespace1"));
                var childNamespace = new _NamespaceDocumentation(parentNamespace: globalNamespace, namespaceId: new NamespaceId("Namespace2"));

                // ACT 
                var ex = Record.Exception(() => sut.Add(@namespace: childNamespace));

                // ASSERT
                var argumentException = Assert.IsType<InconsistentModelException>(ex);
                Assert.Contains("Cannot add namespace 'Namespace2' as a child of namespace 'Namespace1' because the parent namespace is different from the current instance", argumentException.Message);
            }

            [Fact]
            public void Throws_InconsistentModelException_if_namespace_being_added_is_not_a_child_of_the_current_namespace_02()
            {
                // ARRANGE
                var globalNamespace = new _NamespaceDocumentation(null, NamespaceId.GlobalNamespace);
                var sut = globalNamespace;
                var childNamespace = new _NamespaceDocumentation(
                    parentNamespace: new _NamespaceDocumentation(globalNamespace, new NamespaceId("Namespace1")),
                    namespaceId: new NamespaceId("Namespace1.Namespace2")
                );

                // ACT 
                var ex = Record.Exception(() => sut.Add(@namespace: childNamespace));

                // ASSERT
                var argumentException = Assert.IsType<InconsistentModelException>(ex);
                Assert.Contains("Cannot add namespace 'Namespace1.Namespace2' as a child of namespace '<GlobalNamespace>' because the parent namespace is different from the current instance", argumentException.Message);
            }

            [Fact]
            public void Adds_child_namespace()
            {
                // ARRANGE
                var globalNamespace = new _NamespaceDocumentation(null, NamespaceId.GlobalNamespace);
                var sut = new _NamespaceDocumentation(parentNamespace: globalNamespace, namespaceId: new NamespaceId("Namespace1"));
                var childNamespace = new _NamespaceDocumentation(parentNamespace: sut, namespaceId: new NamespaceId("Namespace1.Namespace2"));

                // ACT 
                sut.Add(@namespace: childNamespace);

                // ASSERT
                Assert.Collection(
                    sut.Namespaces,
                    ns => Assert.Same(childNamespace, ns)
                );
            }
        }

        public class Add_Type
        {
            [Fact]
            public void Throws_ArgumentNullException_if_type_is_null()
            {
                // ARRANGE
                var globalNamespace = new _NamespaceDocumentation(null, NamespaceId.GlobalNamespace);
                var sut = new _NamespaceDocumentation(globalNamespace, new NamespaceId("Namespace1"));

                // ACT 
                var ex = Record.Exception(() => sut.Add(type: null!));

                // ASSERT
                var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("type", argumentNullException.ParamName);
            }

            [Fact]
            public void Throws_DuplicateItemException_if_type_already_exists()
            {
                // ARRANGE
                var globalNamespace = new _NamespaceDocumentation(null, NamespaceId.GlobalNamespace);
                var sut = new _NamespaceDocumentation(globalNamespace, new NamespaceId("Namespace1"));

                var class1 = new TypeDocumentationBuilder().AddType(
                    new AssemblyDocumentationBuilder().AddAssembly("Assembly1", null),
                    sut,
                    new SimpleTypeId("Namespace1", "Class1")
                );
                var class2 = new TypeDocumentationBuilder().AddType(
                    new AssemblyDocumentationBuilder().AddAssembly("Assembly1", null),
                    sut,
                    new SimpleTypeId("Namespace1", "Class1")
                );

                // ACT
                sut.Add(class1);
                var ex = Record.Exception(() => sut.Add(class2));

                // ASSERT
                Assert.IsType<DuplicateItemException>(ex);
                Assert.Contains("Type 'Class1' already exists", ex.Message);
            }


            [Fact]
            public void Throws_InconsistentModelException_if_the_types_does_not_match_the_current_namespace()
            {
                // ARRANGE
                var globalNamespace = new _NamespaceDocumentation(null, NamespaceId.GlobalNamespace);
                var sut = new _NamespaceDocumentation(globalNamespace, new NamespaceId("Namespace1"));

                var assembly = new AssemblyDocumentationBuilder().AddAssembly("Assembly1", null);

                var type = new TypeDocumentationBuilder().AddType(assembly, globalNamespace, new SimpleTypeId(NamespaceId.GlobalNamespace, "Class1"));

                // ACT
                var ex = Record.Exception(() => sut.Add(type));

                // ASSERT
                Assert.IsType<InconsistentModelException>(ex);
                Assert.Contains("Cannot add type 'Class1' to namespace 'Namespace1' because the type's namespace is different from the current instance", ex.Message);
            }


            [Fact]
            public void Adds_type()
            {
                // ARRANGE
                var globalNamespace = new _NamespaceDocumentation(null, NamespaceId.GlobalNamespace);
                var sut = new _NamespaceDocumentation(globalNamespace, new NamespaceId("Namespace1"));

                var typeBuilder = new TypeDocumentationBuilder();
                var assemblyBuilder = new AssemblyDocumentationBuilder();

                var class1 = typeBuilder.AddType(assemblyBuilder.GetOrAddAssembly("Assembly1", null), sut, new SimpleTypeId("Namespace1", "Class1"));
                var class2 = typeBuilder.AddType(assemblyBuilder.GetOrAddAssembly("Assembly2", null), sut, new SimpleTypeId("Namespace1", "Class2"));

                // ACT
                sut.Add(class1);
                sut.Add(class2);

                // ASSERT
                Assert.Collection(
                    sut.Types.OrderBy(x => x.DisplayName),
                    type => Assert.Same(class1, type),
                    type => Assert.Same(class2, type)
                );
            }
        }
    }
}
