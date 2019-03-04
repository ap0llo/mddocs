using System;
using System.Collections.Generic;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public abstract class SimpleMemberDocumentation : MemberDocumentation, IObsoleteableDocumentation
    {
        public MemberId MemberId { get; }

        public TextBlock Summary { get; }

        public TextBlock Remarks { get; }

        public IReadOnlyList<SeeAlsoElement> SeeAlso { get; }

        public abstract string Name { get; }

        public abstract string CSharpDefinition { get; }

        public abstract TypeId Type { get; }

        public TextBlock Example { get; }

        public bool IsObsolete { get; }

        public string ObsoleteMessage { get; }


        internal SimpleMemberDocumentation(TypeDocumentation typeDocumentation, MemberId memberId, IXmlDocsProvider xmlDocsProvider, ICustomAttributeProvider definitionAttributes)
            : base(typeDocumentation)
        {
            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));
            definitionAttributes = definitionAttributes ?? throw new ArgumentNullException(nameof(definitionAttributes));

            var documentationComments = xmlDocsProvider.TryGetDocumentationComments(memberId);
            Summary = documentationComments?.Summary;
            Remarks = documentationComments?.Remarks;
            SeeAlso = documentationComments?.SeeAlso?.ToReadOnly() ?? Array.Empty<SeeAlsoElement>();
            Example = documentationComments?.Example;

            IsObsolete = definitionAttributes.IsObsolete(out var obsoleteMessage);
            ObsoleteMessage = obsoleteMessage;
        }


        public override IDocumentation TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : TypeDocumentation.TryGetDocumentation(id);
    }
}
