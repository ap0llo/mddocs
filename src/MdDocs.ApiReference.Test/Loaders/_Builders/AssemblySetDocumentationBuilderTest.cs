using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grynwald.MdDocs.ApiReference.Loaders;
using Grynwald.MdDocs.ApiReference.Model;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Loaders
{
    /// <summary>
    /// Tests for <see cref="AssemblySetDocumentationBuilder"/>
    /// </summary>
    public class AssemblySetDocumentationBuilderTest
    {
        public class Build
        {
            [Fact]
            public void Returns_empty_set_if_no_assemblies_or_types_were_added()
            {
                // ARRANGE
                var sut = new AssemblySetDocumentationBuilder();

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
                var sut = new AssemblySetDocumentationBuilder();

                // ACT
                _ = sut.GetOrAddAssembly("Assembly1", "1.2.3");
                _ = sut.GetOrAddAssembly("Assembly2", "4.5.6");
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
