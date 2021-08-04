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
    /// Test for <see cref="TypeDocumentationBuilder"/>
    /// </summary>
    public class TypeDocumentationBuilderTest
    {

        [Fact]
        public void Types_is_initially_empty()
        {
            // ARRANGE
            var sut = new TypeDocumentationBuilder();

            // ACT 

            // ASSERT
            Assert.Empty(sut.Types);
        }


        [Fact]
        public void AddType_returns_expected_type_and_adds_it_to_the_list_of_types()
        {
            // ARRANGE
            var assemblyBuilder = new AssemblyDocumentationBuilder();
            var assembly = assemblyBuilder.GetOrAddAssembly("Assembly1", null);
            var namespaceBuilder = new NamespaceDocumentationBuilder();
            var @namespace = namespaceBuilder.GetOrAddNamespace("Namespace1");
            var typeId = new SimpleTypeId("Namespace1", "Class1");

            var sut = new TypeDocumentationBuilder();

            // ACT
            var type = sut.AddType(assembly, @namespace, typeId);

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
            var assemblyBuilder = new AssemblyDocumentationBuilder();
            var assembly = assemblyBuilder.GetOrAddAssembly("Assembly1", null);
            var namespaceBuilder = new NamespaceDocumentationBuilder();
            var @namespace = namespaceBuilder.GetOrAddNamespace("Namespace1");
            var typeId = new SimpleTypeId("Namespace1", "Class1");

            var sut = new TypeDocumentationBuilder();

            _ = sut.AddType(assembly, @namespace, typeId);

            // ACT
            var ex = Record.Exception(() => sut.AddType(assembly, @namespace, typeId));

            // ASSERT
            Assert.IsType<DuplicateItemException>(ex);
            Assert.Contains("Type 'Namespace1.Class1' already exists", ex.Message);
        }

        [Fact]
        public void GetOrAddType_returns_expected_type_and_adds_it_to_the_list_of_types()
        {
            // ARRANGE
            var assemblyBuilder = new AssemblyDocumentationBuilder();
            var assembly = assemblyBuilder.GetOrAddAssembly("Assembly1", null);
            var namespaceBuilder = new NamespaceDocumentationBuilder();
            var @namespace = namespaceBuilder.GetOrAddNamespace("Namespace1");
            var typeId = new SimpleTypeId("Namespace1", "Class1");

            var sut = new TypeDocumentationBuilder();

            // ACT
            var type = sut.GetOrAddType(assembly, @namespace, typeId);

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
        public void GetOrAddType_returns_existing_instance_if_type_already_exists()
        {
            // ARRANGE
            var assemblyBuilder = new AssemblyDocumentationBuilder();
            var assembly = assemblyBuilder.GetOrAddAssembly("Assembly1", null);
            var namespaceBuilder = new NamespaceDocumentationBuilder();
            var @namespace = namespaceBuilder.GetOrAddNamespace("Namespace1");
            var typeId = new SimpleTypeId("Namespace1", "Class1");

            var sut = new TypeDocumentationBuilder();

            // ACT
            var type1 = sut.GetOrAddType(assembly, @namespace, typeId);
            var type2 = sut.GetOrAddType(assembly, @namespace, typeId);

            // ASSERT
            Assert.Same(type1, type2);
            Assert.Collection(
                sut.Types,
                t => Assert.Same(type1, t)
            );
        }
    }
}
