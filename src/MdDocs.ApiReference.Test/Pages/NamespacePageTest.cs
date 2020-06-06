using System.Linq;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.ApiReference.Pages;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Test.Pages
{
    /// <summary>
    /// Tests for <see cref="NamespacePage"/>
    /// </summary>
    public class NamespacePageTest : PageTestBase<NamespaceDocumentation, NamespacePage>
    {
        protected override NamespaceDocumentation CreateSampleModel()
        {
            var assembly = Compile(@"
                namespace MyNamespace
                {
                    public class Class1
                    { }
                }
            ");

            var assemblyDocumentation = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);
            return assemblyDocumentation.MainModuleDocumentation.Namespaces.Single();
        }

        protected override NamespacePage CreatePage(NamespaceDocumentation model, ApiReferenceConfiguration configuration)
        {
            return new NamespacePage(NullLinkProvider.Instance, configuration, model, NullLogger.Instance);
        }
    }
}
