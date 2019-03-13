using System;
using System.Collections.Generic;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.Utilities.Collections;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Base documentation model for non-overloadable type members (fields, events, properties)
    /// </summary>
    public abstract class SimpleMemberDocumentation : MemberDocumentation, IObsoleteableDocumentation
    {
        /// <summary>
        /// Gets the id of the member.
        /// </summary>
        public MemberId MemberId { get; }

        /// <summary>
        /// Gets the <c>summary</c> documentation for this member.
        /// </summary>
        public TextBlock Summary { get; }

        /// <summary>
        /// Gets the <c>remarks</c> documentation for this member.
        /// </summary>
        public TextBlock Remarks { get; }

        /// <summary>
        /// Gets the <c>seealso</c> documentation items for this member.
        /// </summary>
        public IReadOnlyList<SeeAlsoElement> SeeAlso { get; }

        /// <summary>
        /// Gets this member's name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the definition of the member as C# code.
        /// </summary>
        public abstract string CSharpDefinition { get; }

        /// <summary>
        /// Gets this member's type.
        /// </summary>
        public abstract TypeId Type { get; }

        /// <summary>
        /// Gets the <c>example</c> documentation for this member.
        /// </summary>
        public TextBlock Example { get; }

        /// <inheritdoc />
        public bool IsObsolete { get; }

        /// <inheritdoc />
        public string ObsoleteMessage { get; }


        // private protected constructor => prevent implementation outside of this assembly
        private protected SimpleMemberDocumentation(TypeDocumentation typeDocumentation, MemberId memberId,
                                                    IXmlDocsProvider xmlDocsProvider,
                                                    ICustomAttributeProvider definitionAttributes) : base(typeDocumentation)
        {
            if (xmlDocsProvider == null)
                throw new ArgumentNullException(nameof(xmlDocsProvider));

            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));
            definitionAttributes = definitionAttributes ?? throw new ArgumentNullException(nameof(definitionAttributes));

            var documentationComments = xmlDocsProvider.TryGetDocumentationComments(memberId);
            Summary = documentationComments?.Summary;
            Remarks = documentationComments?.Remarks;
            SeeAlso = documentationComments?.SeeAlso?.AsReadOnlyList() ?? Array.Empty<SeeAlsoElement>();
            Example = documentationComments?.Example;

            IsObsolete = definitionAttributes.IsObsolete(out var obsoleteMessage);
            ObsoleteMessage = obsoleteMessage;
        }


        /// <inheritdoc />
        public override IDocumentation TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : TypeDocumentation.TryGetDocumentation(id);
    }
}
