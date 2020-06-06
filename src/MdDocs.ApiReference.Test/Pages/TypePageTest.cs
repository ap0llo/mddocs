using System.Linq;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.ApiReference.Pages;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Test.Pages
{
    /// <summary>
    /// Tests for <see cref="TypePage"/>
    /// </summary>
    public class TypePageTest : PageTestBase<TypeDocumentation, TypePage>
    {

        protected override TypeDocumentation CreateSampleModel()
        {
            var assembly = Compile(@"
                namespace MyNamespace
                {
                    public class Class1
                    { }
                }
            ");

            var assemblyDocumentation = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);
            return assemblyDocumentation.MainModuleDocumentation.Types.Single();
        }

        protected override TypePage CreatePage(TypeDocumentation model, ApiReferenceConfiguration configuration)
        {
            return new TypePage(NullLinkProvider.Instance, configuration, model, NullLogger.Instance);
        }
    }
}
