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
    /// Tests for <see cref="_AssemblyDocumentation"/>
    /// </summary>
    public class _AssemblyDocumentationTest
    {
        // TODO 2021-08-04: Port over tests from AssemblyDocumentationTest if applicable

        public class Add
        {
            [Fact]
            public void Throws_ArgumentNullException_if_type_is_null()
            {
                // ARRANGE
                var sut = new _AssemblyDocumentation("Assembly", "1.0.0");

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
                var sut = new _AssemblyDocumentation("Assembly", "1.0.0");

                var namespaceBuilder = new NamespaceDocumentationBuilder();

                var class1 = new TypeDocumentationBuilder().AddType(sut, namespaceBuilder.GetOrAddNamespace("Namespace1"), new SimpleTypeId("Namespace1", "Class1"));
                var class2 = new TypeDocumentationBuilder().AddType(sut, namespaceBuilder.GetOrAddNamespace("Namespace1"), new SimpleTypeId("Namespace1", "Class1"));

                // ACT
                sut.Add(class1);
                var ex = Record.Exception(() => sut.Add(class2));

                // ASSERT
                Assert.IsType<DuplicateItemException>(ex);
                Assert.Contains("Type 'Namespace1.Class1' already exists", ex.Message);
            }

            [Fact]
            public void Adds_type()
            {
                // ARRANGE
                var sut = new _AssemblyDocumentation("Assembly", "1.0.0");

                var typeBuilder = new TypeDocumentationBuilder();
                var namespaceBuilder = new NamespaceDocumentationBuilder();

                var class1 = typeBuilder.AddType(sut, namespaceBuilder.GetOrAddNamespace("Namespace1"), new SimpleTypeId("Namespace1", "Class1"));
                var class2 = typeBuilder.AddType(sut, namespaceBuilder.GetOrAddNamespace("Namespace2"), new SimpleTypeId("Namespace2", "Class2"));

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
