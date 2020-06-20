using System;
using System.Collections.Generic;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.Common.Templates;
using Grynwald.Utilities.Collections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.ApiReference.Templates.Default
{
    public class ApiReferenceDefaultTemplate : ITemplate<AssemblyDocumentation>
    {
        private readonly ApiReferenceConfiguration m_Configuration;

        public ApiReferenceDefaultTemplate(ApiReferenceConfiguration configuration)
        {
            m_Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        public DocumentSet<IDocument> Render(AssemblyDocumentation model)
        {
            var pageFactory = new PageFactory(new DefaultApiReferencePathProvider(), m_Configuration, model);
            return pageFactory.GetPages();
        }
    }
}
