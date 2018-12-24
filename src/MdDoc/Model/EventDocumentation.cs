using System;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public class EventDocumentation : MemberDocumentation
    {
        public string Name => Definition.Name;

        public MemberId MemberId { get; }

        public TextBlock Summary { get; }

        public string CSharpDefinition { get; }

        internal EventDefinition Definition { get; }


        internal EventDocumentation(TypeDocumentation typeDocumentation, EventDefinition definition, IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            MemberId = definition.ToMemberId();
            Summary = xmlDocsProvider.TryGetDocumentationComments(MemberId)?.Summary;
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);
        }

        public override IDocumentation TryGetDocumentation(MemberId id) => 
            MemberId.Equals(id) ? this : TypeDocumentation.TryGetDocumentation(id);
    }
}
