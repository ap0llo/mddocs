using System;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.TestHelpers;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    /// <summary>
    /// Tests for <see cref="_AssemblySetDocumentation"/>
    /// </summary>
    public class _AssemblySetDocumentationTest : DynamicCompilationTestBase
    {
        [Fact]
        public void Constructor_throws_ArgumentNullException_if_assemblies_parameter_is_null()
        {
            // ARRANGE
            var namespaces = Array.Empty<_NamespaceDocumentation>();
            var types = Array.Empty<_TypeDocumentation>();

            // ACT 
            var ex = Record.Exception(() => new _AssemblySetDocumentation(assemblies: null!, namespaces: namespaces, types: types));

            // ASSERT
            var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal("assemblies", argumentNullException.ParamName);
        }

        [Fact]
        public void Constructor_throws_ArgumentNullException_if_namespaces_parameter_is_null()
        {
            // ARRANGE
            var assemblies = Array.Empty<_AssemblyDocumentation>();
            var types = Array.Empty<_TypeDocumentation>();

            // ACT 
            var ex = Record.Exception(() => new _AssemblySetDocumentation(assemblies: assemblies, namespaces: null!, types: types));

            // ASSERT
            var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal("namespaces", argumentNullException.ParamName);
        }

        [Fact]
        public void Constructor_throws_ArgumentNullException_if_types_parameter_is_null()
        {
            // ARRANGE
            var assemblies = Array.Empty<_AssemblyDocumentation>();
            var namespaces = Array.Empty<_NamespaceDocumentation>();

            // ACT 
            var ex = Record.Exception(() => new _AssemblySetDocumentation(assemblies: assemblies, namespaces: namespaces, types: null!));

            // ASSERT
            var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal("types", argumentNullException.ParamName);
        }

        [Theory]
        [InlineData("Assembly1", "Assembly1")]
        [InlineData("Assembly1", "assembly1")]
        [InlineData("ASSEMBLY1", "Assembly1")]
        public void Constructor_throws_DuplicateItemException_if_assemblies_contains_duplicate_items(string assemblyName1, string assemblyName2)
        {
            // ARRANGE
            var assembly1 = new _AssemblyDocumentation(assemblyName1, null);
            var assembly2 = new _AssemblyDocumentation(assemblyName2, null);

            var assemblies = new[] { assembly1, assembly2 };
            var namespaces = Array.Empty<_NamespaceDocumentation>();
            var types = Array.Empty<_TypeDocumentation>();

            // ACT
            var ex = Record.Exception(() => new _AssemblySetDocumentation(assemblies, namespaces, types));

            // ASSERT
            Assert.IsType<DuplicateItemException>(ex);
            Assert.Contains($"multiple assemblies named {assemblyName1}", ex.Message, StringComparison.OrdinalIgnoreCase);
        }


        // TODO 2021-08-04: Constructor_throws_InvalidAssemblySetException_if_namesapces_contains_duplicate_items
        // TODO 2021-08-04: Constructor_throws_InvalidAssemblySetException_if_types_contains_duplicate_items
        // TODO 2021-08-04: Constructor_throws_InvalidAssemblySetException_if_a_type_exists_in_multiple_assemblies
        // TODO 2021-08-04: Constructor_throws_InvalidAssemblySetException_if_types_contains_items_not_found_in_assemblies
        // TODO 2021-08-04: Constructor_throws_InvalidAssemblySetException_if_types_contains_items_not_found_in_namespaces
        // TODO 2021-08-04: Constructor_throws_InvalidAssemblySetException_if_namespaces_contains_items_not_found_in_assemblies
    }
}
