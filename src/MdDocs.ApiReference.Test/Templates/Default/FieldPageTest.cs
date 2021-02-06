using System.Linq;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.ApiReference.Templates.Default;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Test.Templates.Default
{
    /// <summary>
    /// Tests for <see cref="FieldPage" />
    /// </summary>
    public class FieldPageTest : PageTestBase<FieldDocumentation, FieldPage>
    {
        protected override FieldPage CreatePage(FieldDocumentation model, ApiReferenceConfiguration configuration)
        {
            return new FieldPage(NullLinkProvider.Instance, configuration, model, NullLogger.Instance);
        }

        protected override FieldDocumentation CreateSampleModel()
        {
            var assembly = Compile(@"
                namespace MyNamespace
                {
                    public class Class1
                    {
                        public int Field1;
                    }
                }
            ");

            var assemblyDocumentation = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);
            return assemblyDocumentation.Types.Single().Fields.Single();
        }
    }
}
