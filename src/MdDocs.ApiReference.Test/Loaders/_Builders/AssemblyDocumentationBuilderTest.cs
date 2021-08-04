using System;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Loaders;
using Grynwald.MdDocs.ApiReference.Model;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Loaders
{
    /// <summary>
    /// Tests for <see cref="AssemblyDocumentationBuilder"/>
    /// </summary>
    public class AssemblyDocumentationBuilderTest
    {
        [Fact]
        public void Assemblies_is_initially_empty()
        {
            // ARRANGE
            var sut = new AssemblyDocumentationBuilder();

            // ACT 

            // ASSERT
            Assert.Empty(sut.Assemblies);
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
                var sut = new AssemblyDocumentationBuilder();

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
                var sut = new AssemblyDocumentationBuilder();

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
                var sut = new AssemblyDocumentationBuilder();

                // ACT 
                _ = sut.AddAssembly(assemblyName: "SomeAssemblyName", assemblyVersion: "1.0.0");
                var ex = Record.Exception(() => sut.AddAssembly(assemblyName: "someassemblyname", assemblyVersion: "1.0.0"));

                // ASSERT
                Assert.IsType<DuplicateItemException>(ex);
                Assert.Contains("Assembly 'someassemblyname' already exists", ex.Message);
            }
        }


        public class GetOrAddAssembly
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData(" ")]
            [InlineData("\t")]
            public void Throws_ArgumentException_when_assembly_name_is_null_or_whitespace(string assemblyName)
            {
                // ARRANGE
                var sut = new AssemblyDocumentationBuilder();

                // ACT
                var ex = Record.Exception(() => sut.GetOrAddAssembly(assemblyName: assemblyName, assemblyVersion: "1.0.0"));

                // ASSERT
                var argumentException = Assert.IsType<ArgumentException>(ex);
                Assert.Equal("assemblyName", argumentException.ParamName);
            }

            [Fact]
            public void Returns_new_assembly()
            {
                // ARRANGE
                var sut = new AssemblyDocumentationBuilder();

                // ACT 
                var addedAssembly1 = sut.GetOrAddAssembly("SomeAssemblyName1", assemblyVersion: null);
                var addedAssembly2 = sut.GetOrAddAssembly("SomeAssemblyName2", assemblyVersion: "1.0.0");

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
            public void Returns_existing_assembly_if_it_already_exists()
            {
                // ARRANGE
                var sut = new AssemblyDocumentationBuilder();

                // ACT 
                var addedAssembly1 = sut.GetOrAddAssembly(assemblyName: "SomeAssemblyName", assemblyVersion: "1.0.0");
                var addedAssembly2 = sut.GetOrAddAssembly(assemblyName: "someassemblyname", assemblyVersion: "1.0.0");

                // ASSERT
                Assert.Same(addedAssembly1, addedAssembly2);
                Assert.Equal("1.0.0", addedAssembly1.Version);
                Assert.Collection(
                    sut.Assemblies.OrderBy(x => x.Name),
                    assembly => Assert.Same(addedAssembly1, assembly)
                );
            }
        }


    }
}


