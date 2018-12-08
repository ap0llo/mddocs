using System;
using MdDoc.XmlDocs;
using Mono.Cecil;
using NuDoq;

namespace MdDoc.Model
{
    public sealed class MethodOverloadDocumentation : OverloadDocumentation
    {
        private readonly IXmlDocsProvider m_XmlDocsProvider;

        public string MethodName => Definition.Name;

        public MethodDocumentation MethodDocumentation { get; }

        public Summary Summary { get; }


        internal MethodOverloadDocumentation(MethodDocumentation methodDocumentation, MethodDefinition definition, IXmlDocsProvider xmlDocsProvider) : base(definition)
        {
            MethodDocumentation = methodDocumentation ?? throw new ArgumentNullException(nameof(methodDocumentation));
            m_XmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));

            Summary = m_XmlDocsProvider.TryGetSummary(definition);
        }


        public override TypeDocumentation TryGetDocumentation(TypeName type) => 
            MethodDocumentation.TryGetDocumentation(type);
    }
}
