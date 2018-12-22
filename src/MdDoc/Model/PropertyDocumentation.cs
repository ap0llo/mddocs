using System;
using System.Linq;
using System.Text;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public class PropertyDocumentation : MemberDocumentation
    {
        public MemberId MemberId { get; }

        public string Name => Definition.Name;

        public TypeId PropertyType { get; }

        // Indexeres are modeled as properties with parameters
        public bool IsIndexer => Definition.HasParameters;
        
        public string CSharpDefinition { get; }

        public TextBlock Summary { get; }

        internal PropertyDefinition Definition { get; }


        internal PropertyDocumentation(TypeDocumentation typeDocumentation, PropertyDefinition definition, IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            PropertyType = definition.PropertyType.ToTypeId();
            MemberId = definition.ToMemberId();
            Summary = xmlDocsProvider.TryGetDocumentationComments(MemberId)?.Summary;
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);
        }


        public override IDocumentation TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : TypeDocumentation.TryGetDocumentation(id);
    }
}
