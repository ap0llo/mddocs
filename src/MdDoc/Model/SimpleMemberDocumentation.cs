using MdDoc.Model.XmlDocs;

namespace MdDoc.Model
{
    public abstract class SimpleMemberDocumentation : MemberDocumentation
    {
        public MemberId MemberId { get; }

        public TextBlock Summary { get; }

        public abstract string Name { get; }

        public abstract string CSharpDefinition { get; }

        public abstract TypeId Type { get; }


        internal SimpleMemberDocumentation(TypeDocumentation typeDocumentation, MemberId memberId, IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation)
        {
            MemberId = memberId ?? throw new System.ArgumentNullException(nameof(memberId));
            Summary = xmlDocsProvider.TryGetDocumentationComments(memberId)?.Summary;
        }


        public override IDocumentation TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : TypeDocumentation.TryGetDocumentation(id);
    }
}
