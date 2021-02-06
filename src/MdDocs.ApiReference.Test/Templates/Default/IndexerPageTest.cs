using System.Linq;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.ApiReference.Templates.Default;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Test.Templates.Default
{
    /// <summary>
    /// Tests for <see cref="IndexerPage" />
    /// </summary>
    public class IndexerPageTest : PageTestBase<IndexerDocumentation, IndexerPage>
    {
        protected override IndexerPage CreatePage(IndexerDocumentation model, ApiReferenceConfiguration configuration)
        {
            return new IndexerPage(NullLinkProvider.Instance, configuration, model, NullLogger.Instance);
        }

        protected override IndexerDocumentation CreateSampleModel()
        {
            var assembly = Compile(@"
                namespace MyNamespace
                {
                    public class Class1
                    {
                        public int this[int i] { get { return 0; } }
                    }
                }
            ");

            using var assemblySetDocumentation = AssemblySetDocumentation.FromAssemblyDefinitions(assembly);
            var assemblyDocumentation = assemblySetDocumentation.Assemblies.Single();
            return assemblyDocumentation.Types.Single().Indexers.Single();
        }
    }
}
