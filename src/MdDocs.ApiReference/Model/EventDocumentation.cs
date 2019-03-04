using System;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public class EventDocumentation : SimpleMemberDocumentation
    {
        public override string Name => Definition.Name;

        public override string CSharpDefinition { get; }

        public override TypeId Type { get; }

        internal EventDefinition Definition { get; }


        internal EventDocumentation(TypeDocumentation typeDocumentation, EventDefinition definition, IXmlDocsProvider xmlDocsProvider)
            : base(typeDocumentation, definition?.ToMemberId(), xmlDocsProvider, definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);
            Type = definition.EventType.ToTypeId();
        }
    }
}
