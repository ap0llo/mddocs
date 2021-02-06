using System.Linq;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.ApiReference.Templates.Default;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Test.Templates.Default
{
    /// <summary>
    /// Tests for <see cref="OperatorPage" />
    /// </summary>
    public class OperatorPageTest : PageTestBase<OperatorDocumentation, OperatorPage>
    {
        protected override OperatorPage CreatePage(OperatorDocumentation model, ApiReferenceConfiguration configuration)
        {
            return new OperatorPage(NullLinkProvider.Instance, configuration, model, NullLogger.Instance);
        }

        protected override OperatorDocumentation CreateSampleModel()
        {
            var assembly = Compile(@"
                using System;

                namespace MyNamespace
                {
                    public class Class1
                    {
                        public static int operator -(Class1 a, Class1 b) => throw new NotImplementedException();
                    }
                }
            ");

            var assemblyDocumentation = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);
            return assemblyDocumentation.Types.Single().Operators.Single();
        }
    }
}
