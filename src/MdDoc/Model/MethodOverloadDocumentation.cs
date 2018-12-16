using System;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public sealed class MethodOverloadDocumentation : OverloadDocumentation
    {
        private readonly IXmlDocsProvider m_XmlDocsProvider;

        public string MethodName => Definition.Name;

        public MethodDocumentation MethodDocumentation { get; }

        public SummaryElement Summary { get; }


        internal MethodOverloadDocumentation(MethodDocumentation methodDocumentation, MethodDefinition definition, IXmlDocsProvider xmlDocsProvider) : base(definition)
        {
            MethodDocumentation = methodDocumentation ?? throw new ArgumentNullException(nameof(methodDocumentation));
            m_XmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));

            Summary = m_XmlDocsProvider.TryGetDocumentationComments(definition)?.Summary;
        }


        public override TypeDocumentation TryGetDocumentation(TypeName type) => 
            MethodDocumentation.TryGetDocumentation(type);
    }
}
