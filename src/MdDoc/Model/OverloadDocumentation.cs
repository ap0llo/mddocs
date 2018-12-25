using System;
using System.Collections.Generic;
using System.Linq;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public abstract class OverloadDocumentation : IDocumentation
    {
        public MemberId MemberId { get; }

        public TextBlock Summary { get; }

        public abstract string Signature { get; }

        public abstract IReadOnlyList<ParameterDocumentation> Parameters { get; }

        public abstract string CSharpDefinition { get; }     


        internal OverloadDocumentation(MemberId memberId, IXmlDocsProvider xmlDocsProvider)
        {
            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));
            Summary = xmlDocsProvider.TryGetDocumentationComments(memberId)?.Summary;
        }


        public abstract IDocumentation TryGetDocumentation(MemberId id);
    }
}
