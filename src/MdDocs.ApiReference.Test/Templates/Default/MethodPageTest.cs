using System.Linq;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.ApiReference.Templates.Default;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Test.Templates.Default
{
    /// <summary>
    /// Tests for <see cref="MethodPage" />
    /// </summary>
    public class MethodPageTest : PageTestBase<MethodDocumentation, MethodPage>
    {
        protected override MethodPage CreatePage(MethodDocumentation model, ApiReferenceConfiguration configuration)
        {
            return new MethodPage(NullLinkProvider.Instance, configuration, model, NullLogger.Instance);
        }

        protected override MethodDocumentation CreateSampleModel()
        {
            var assembly = Compile(@"
                namespace MyNamespace
                {
                    public class Class1
                    {
                        public void Method1()
                        { }
                    }
                }
            ");

            var assemblyDocumentation = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);
            return assemblyDocumentation.Types.Single().Methods.Single();
        }
    }
}
