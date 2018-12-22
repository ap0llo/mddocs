using System;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public class FieldDocumentation : MemberDocumentation
    {
        public string Name => Definition.Name;

        public MemberId MemberId { get; }       

        public TextBlock Summary { get; }

        internal FieldDefinition Definition { get; }
        

        internal FieldDocumentation(TypeDocumentation typeDocumentation, FieldDefinition definition, IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            MemberId = definition.ToMemberId();
            Summary = xmlDocsProvider.TryGetDocumentationComments(MemberId)?.Summary;
        }


        public override IDocumentation TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : TypeDocumentation.TryGetDocumentation(id);
    }
}
