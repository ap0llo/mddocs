using System.Linq;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.ApiReference.Templates.Default;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Test.Templates.Default
{
    /// <summary>
    /// Tests for <see cref="PropertyPage" />
    /// </summary>
    public class PropertyPageTest : PageTestBase<PropertyDocumentation, PropertyPage>
    {
        protected override PropertyPage CreatePage(PropertyDocumentation model, ApiReferenceConfiguration configuration)
        {
            return new PropertyPage(NullLinkProvider.Instance, configuration, model, NullLogger.Instance);
        }

        protected override PropertyDocumentation CreateSampleModel()
        {
            var assembly = Compile(@"
                namespace MyNamespace
                {
                    public class Class1
                    {
                        public int Property1 { get; set; }
                    }
                }
            ");

            var assemblyDocumentation = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);
            return assemblyDocumentation.MainModuleDocumentation.Types.Single().Properties.Single();

        }
    }
}
