using System;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Loaders;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Loaders
{
    /// <summary>
    /// Tests for <see cref="NamespaceDocumentationBuilder"/>
    /// </summary>
    public class NamespaceDocumentationBuilderTest
    {
        [Fact]
        public void Namespaces_is_empty_when_no_namespace_is_added()
        {
            // ARRANGE
            var sut = new NamespaceDocumentationBuilder();

            // ACT 

            // ASSERT
            Assert.Empty(sut.Namespaces);
        }

        [Fact]
        public void Adding_the_global_namespace_returns_singleton_instance()
        {
            // ARRANGE
            var sut = new NamespaceDocumentationBuilder();

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
        public void Adding_a_namespace_implicitly_adds_the_global_namespace()
        {
            // ARRANGE
            var sut = new NamespaceDocumentationBuilder();

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
        public void Adding_a_namespace_implicitly_adds_all_parent_namespaces()
        {
            // ARRANGE
            var sut = new NamespaceDocumentationBuilder();

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
        public void Adding_a_namespace_that_already_exists_returns_previous_instance()
        {
            // ARRANGE
            var sut = new NamespaceDocumentationBuilder();

            // ACT 
            var addedNamespace1 = sut.GetOrAddNamespace("Namespace1.Namespace2.Namespace3");
            var addedNamespace2 = sut.GetOrAddNamespace("Namespace1.Namespace2.Namespace3");

            // ASSERT
            Assert.Same(addedNamespace1, addedNamespace2);
        }

        [Fact]
        public void Adding_a_namespace_reuses_parent_namespaces_if_they_were_already_added()
        {
            // ARRANGE
            var sut = new NamespaceDocumentationBuilder();

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
        public void Adding_a_namespace_adds_it_to_the_parent_namespace()
        {
            // ARRANGE
            var sut = new NamespaceDocumentationBuilder();

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
        public void NamespaceName_must_not_be_null_or_whitespace(string namespaceName)
        {
            // ARRANGE
            var sut = new NamespaceDocumentationBuilder();

            // ACT
            var ex = Record.Exception(() => sut.GetOrAddNamespace(namespaceName: namespaceName));

            // ASSERT
            var argumentException = Assert.IsType<ArgumentException>(ex);
            Assert.Equal("namespaceName", argumentException.ParamName);
        }
    }
}
