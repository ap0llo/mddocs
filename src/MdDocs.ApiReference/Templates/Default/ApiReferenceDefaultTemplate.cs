using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.Common.Templates;

namespace Grynwald.MdDocs.ApiReference.Templates.Default
{
    public class ApiReferenceDefaultTemplate : ITemplate<AssemblySetDocumentation>
    {
        private readonly ApiReferenceConfiguration m_Configuration;

        public ApiReferenceDefaultTemplate(ApiReferenceConfiguration configuration)
        {
            m_Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        public DocumentSet<IDocument> Render(AssemblySetDocumentation model)
        {
            var pageFactory = new PageFactory(new DefaultApiReferencePathProvider(), m_Configuration, model);
            return pageFactory.GetPages();
        }
    }
}
