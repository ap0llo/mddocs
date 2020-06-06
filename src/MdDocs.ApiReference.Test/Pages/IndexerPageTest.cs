using System;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.ApiReference.Pages;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Test.Pages
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

            var assemblyDocumentation = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);
            return assemblyDocumentation.MainModuleDocumentation.Types.Single().Indexers.Single();
        }
    }
}
