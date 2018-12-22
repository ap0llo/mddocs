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

        public TextBlock Summary { get; }


        internal MethodOverloadDocumentation(MethodDocumentation methodDocumentation, MethodDefinition definition, IXmlDocsProvider xmlDocsProvider) : base(definition)
        {
            MethodDocumentation = methodDocumentation ?? throw new ArgumentNullException(nameof(methodDocumentation));
            m_XmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));

            Summary = m_XmlDocsProvider.TryGetDocumentationComments(MemberId)?.Summary;
        }


        public override IDocumentation TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : MethodDocumentation.TryGetDocumentation(id);
    }
}
