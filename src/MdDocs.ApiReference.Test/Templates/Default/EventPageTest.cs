using System.Linq;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.ApiReference.Templates.Default;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Test.Templates.Default
{
    /// <summary>
    /// Tests for <see cref="EventPage" />
    /// </summary>
    public class EventPageTest : PageTestBase<EventDocumentation, EventPage>
    {
        protected override EventPage CreatePage(EventDocumentation model, ApiReferenceConfiguration configuration)
        {
            return new EventPage(NullLinkProvider.Instance, configuration, model, NullLogger.Instance);
        }

        protected override EventDocumentation CreateSampleModel()
        {
            var assembly = Compile(@"
                using System;

                namespace MyNamespace
                {
                    public class Class1
                    {
                        public event EventHandler Event1;
                    }
                }
            ");

            var assemblyDocumentation = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);
            return assemblyDocumentation.MainModuleDocumentation.Types.Single().Events.Single();
        }
    }
}
