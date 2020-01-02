using System.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public class AssemblyDocumentationTest : DynamicCompilationTestBase
    {
        [Fact]
        public void TryGetDocumentation_returns_null_for_an_undocumented_type()
        {
            // ARRANGE
            var cs = @"
                namespace Namespace1.Namespace2
                {
                    internal class InternalClass1
                    { }
                }
            ";

            using var assembly = Compile(cs);

            var typeId = assembly.MainModule.Types.Single(x => x.Name == "InternalClass1").ToTypeId();
            var sut = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);

            // ACT
            var documentation = sut.TryGetDocumentation(typeId);

            // ASSERT
            Assert.Null(documentation);
        }

        [Fact]
        public void TryGetDocumentation_returns_expected_documentation_item_for_an_documented_type()
        {
            // ARRANGE
            var cs = @"
                namespace Namespace1.Namespace2
                {
                    public class Class1
                    { }
                }
            ";

            using var assembly = Compile(cs);

            var typeId = assembly.MainModule.Types.Single(x => x.Name == "Class1").ToTypeId();
            var sut = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);

            // ACT
            var documentation = sut.TryGetDocumentation(typeId);

            // ASSERT
            Assert.NotNull(documentation);
            var typeDocumentation = Assert.IsType<TypeDocumentation>(documentation);
            Assert.Equal(typeId, typeDocumentation.TypeId);
        }
    }
}
