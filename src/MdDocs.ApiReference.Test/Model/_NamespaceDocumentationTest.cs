using System;
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
                Assert.IsType<ArgumentException>(ex);
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
                Assert.IsType<ArgumentException>(ex);
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

        public class Add
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
            public void Throws_ArgumentException_if_namespace_being_added_is_not_a_child_of_the_current_namespace_01()
            {
                // ARRANGE
                var globalNamespace = new _NamespaceDocumentation(null, NamespaceId.GlobalNamespace);
                var sut = new _NamespaceDocumentation(parentNamespace: globalNamespace, namespaceId: new NamespaceId("Namespace1"));
                var childNamespace = new _NamespaceDocumentation(parentNamespace: globalNamespace, namespaceId: new NamespaceId("Namespace2"));

                // ACT 
                var ex = Record.Exception(() => sut.Add(@namespace: childNamespace));

                // ASSERT
                var argumentException = Assert.IsType<ArgumentException>(ex);
                Assert.Equal("namespace", argumentException.ParamName);
                Assert.Contains("Cannot add namespace 'Namespace2' as a child of namespace 'Namespace1' because the parent namespace if different from the current instance", argumentException.Message);
            }

            [Fact]
            public void Throws_ArgumentException_if_namespace_being_added_is_not_a_child_of_the_current_namespace_02()
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
                var argumentException = Assert.IsType<ArgumentException>(ex);
                Assert.Equal("namespace", argumentException.ParamName);
                Assert.Contains("Cannot add namespace 'Namespace1.Namespace2' as a child of namespace '<GlobalNamespace>' because the parent namespace if different from the current instance", argumentException.Message);
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
    }
}
