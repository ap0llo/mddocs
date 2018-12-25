using System;
using System.Collections.Generic;
using System.Linq;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public abstract class OverloadDocumentation : IDocumentation
    {
        private readonly MethodFormatter m_MethodFormatter = MethodFormatter.Instance;


        public MemberId MemberId { get; }
        
        public string Signature => m_MethodFormatter.GetSignature(Definition);

        public IReadOnlyList<ParameterDocumentation> Parameters { get; }

        public string CSharpDefinition { get; }

        public TextBlock Summary { get; }

        internal MethodDefinition Definition { get; }


        internal OverloadDocumentation(MethodDefinition definition, IXmlDocsProvider xmlDocsProvider)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            MemberId = definition.ToMemberId();

            Parameters = definition.HasParameters
                ? Array.Empty<ParameterDocumentation>()
                : definition.Parameters.Select(p => new ParameterDocumentation(this, p)).ToArray();

            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);

            Summary = xmlDocsProvider.TryGetDocumentationComments(MemberId)?.Summary;
        }


        public abstract IDocumentation TryGetDocumentation(MemberId id);
    }
}
