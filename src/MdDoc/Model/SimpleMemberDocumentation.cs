using System;
using System.Collections.Generic;
using MdDoc.Model.XmlDocs;

namespace MdDoc.Model
{
    public abstract class SimpleMemberDocumentation : MemberDocumentation
    {
        public MemberId MemberId { get; }

        public TextBlock Summary { get; }

        public IReadOnlyList<SeeAlsoElement> SeeAlso { get; }

        public abstract string Name { get; }

        public abstract string CSharpDefinition { get; }

        public abstract TypeId Type { get; }


        internal SimpleMemberDocumentation(TypeDocumentation typeDocumentation, MemberId memberId, IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation)
        {
            MemberId = memberId ?? throw new System.ArgumentNullException(nameof(memberId));
            var documentationComments = xmlDocsProvider.TryGetDocumentationComments(memberId);
            Summary = documentationComments?.Summary;
            SeeAlso = documentationComments?.SeeAlso ?? Array.Empty<SeeAlsoElement>();
        }


        public override IDocumentation TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : TypeDocumentation.TryGetDocumentation(id);
    }
}
