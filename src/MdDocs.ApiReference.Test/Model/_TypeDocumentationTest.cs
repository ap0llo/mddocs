using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grynwald.MdDocs.ApiReference.Loaders;
using Grynwald.MdDocs.ApiReference.Model;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    /// <summary>
    /// Tests for <see cref="_TypeDocumentation"/>
    /// </summary>
    public class _TypeDocumentationTest
    {
        public class Constructor
        {
            [Fact]
            public void Assembly_must_not_be_null()
            {
                // ARRANGE
                var typeId = new SimpleTypeId("Namespace1", "Class1");
                var namespaceBuilder = new NamespaceDocumentationBuilder();
                var @namespace = namespaceBuilder.GetOrAddNamespace("Namespace1");

                // ACT 
                var ex = Record.Exception(() => new _TypeDocumentation(assembly: null!, @namespace: @namespace, typeId: typeId));

                // ASSERT
                var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("assembly", argumentNullException.ParamName);
            }

            [Fact]
            public void Namespace_must_not_be_null()
            {
                // ARRANGE
                var typeId = new SimpleTypeId("Namespace1", "Class1");
                var assemblyBuilder = new AssemblyDocumentationBuilder();
                var assembly = assemblyBuilder.GetOrAddAssembly("Assembly1", "1.0.0");

                // ACT 
                var ex = Record.Exception(() => new _TypeDocumentation(assembly: assembly, @namespace: null!, typeId: typeId));

                // ASSERT
                var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("namespace", argumentNullException.ParamName);
            }

            [Fact]
            public void TypeId_must_not_be_null()
            {
                // ARRANGE
                var namespaceBuilder = new NamespaceDocumentationBuilder();
                var @namespace = namespaceBuilder.GetOrAddNamespace("Namespace1");
                var assemblyBuilder = new AssemblyDocumentationBuilder();
                var assembly = assemblyBuilder.GetOrAddAssembly("Assembly1", "1.0.0");

                // ACT 
                var ex = Record.Exception(() => new _TypeDocumentation(assembly: assembly, @namespace: @namespace, typeId: null!));

                // ASSERT
                var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("typeId", argumentNullException.ParamName);
            }

            [Fact]
            public void Namespace_of_type_id_has_to_match_namespace()
            {
                // ARRANGE
                var typeId = new SimpleTypeId("Namespace1", "Class1");
                var namespaceBuilder = new NamespaceDocumentationBuilder();
                var @namespace = namespaceBuilder.GetOrAddNamespace("Namespace2");
                var assemblyBuilder = new AssemblyDocumentationBuilder();
                var assembly = assemblyBuilder.GetOrAddAssembly("Assembly1", "1.0.0");

                // ACT 
                var ex = Record.Exception(() => new _TypeDocumentation(assembly, @namespace, typeId));

                // ASSERT
                Assert.IsType<InconsistentModelException>(ex);
                Assert.Contains("Mismatch between namespace of type 'Namespace1.Class1' and id of parent namespace 'Namespace2'", ex.Message);
            }
        }

    }
}
