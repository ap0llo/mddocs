using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.ApiReference.Pages;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Test.Pages
{
    /// <summary>
    /// Tests for <see cref="ConstructorsPage" />
    /// </summary>
    public class ConstructorsPageTest : PageTestBase<ConstructorDocumentation, ConstructorsPage>
    {
        protected override ConstructorsPage CreatePage(ConstructorDocumentation model, ApiReferenceConfiguration configuration)
        {
            return new ConstructorsPage(NullLinkProvider.Instance, configuration, model, NullLogger.Instance);
        }

        protected override ConstructorDocumentation CreateSampleModel()
        {
            var assembly = Compile(@"
                namespace MyNamespace
                {
                    public class Class1
                    {
                        public Class1()
                        { }
                    }
                }
            ");

            var assemblyDocumentation = new AssemblyDocumentation(assembly, NullXmlDocsProvider.Instance, NullLogger.Instance);
            return assemblyDocumentation.MainModuleDocumentation.Types.Single().Constructors!;
        }
    }
}
