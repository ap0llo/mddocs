using System;
using System.Collections.Generic;
using MdDoc.Model.XmlDocs;

namespace MdDoc.Model
{
    public abstract class OverloadDocumentation : IDocumentation
    {
        public MemberId MemberId { get; }

        public TextBlock Summary { get; }

        public TextBlock Remarks { get; }

        public IReadOnlyList<SeeAlsoElement> SeeAlso { get; }

        public abstract string Signature { get; }

        public abstract IReadOnlyList<ParameterDocumentation> Parameters { get; }

        public abstract IReadOnlyList<TypeParameterDocumentation> TypeParameters { get; }

        public abstract string CSharpDefinition { get; }     

        public abstract TypeId Type { get; }

        public TextBlock Returns { get; }


        internal OverloadDocumentation(MemberId memberId, IXmlDocsProvider xmlDocsProvider)
        {
            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));

            var documentationComments = xmlDocsProvider.TryGetDocumentationComments(memberId);
            Summary = documentationComments?.Summary;
            Remarks = documentationComments?.Remarks;
            SeeAlso = documentationComments?.SeeAlso?.ToReadOnly() ?? Array.Empty<SeeAlsoElement>();
            Returns = documentationComments?.Returns;
        }


        public abstract IDocumentation TryGetDocumentation(MemberId id);
    }
}
