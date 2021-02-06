using System.Linq;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.ApiReference.Templates.Default;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Test.Templates.Default
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

            using var assemblySetDocumentation = AssemblySetDocumentation.FromAssemblyDefinitions(assembly);
            var assemblyDocumentation = assemblySetDocumentation.Assemblies.Single();
            return assemblyDocumentation.Namespaces.Single();
        }

        protected override NamespacePage CreatePage(NamespaceDocumentation model, ApiReferenceConfiguration configuration)
        {
            return new NamespacePage(NullLinkProvider.Instance, configuration, model, NullLogger.Instance);
        }
    }
}
