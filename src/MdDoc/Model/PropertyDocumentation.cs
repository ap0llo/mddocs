using System;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public class PropertyDocumentation : SimpleMemberDocumentation
    {        
        public override string Name => Definition.Name;

        public override TypeId Type { get; }

        public override string CSharpDefinition { get; }

        // Indexeres are modeled as properties with parameters
        public bool IsIndexer => Definition.HasParameters;
        
        internal PropertyDefinition Definition { get; }


        internal PropertyDocumentation(
            TypeDocumentation typeDocumentation,
            PropertyDefinition definition,
            IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation, definition?.ToMemberId(), xmlDocsProvider)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Type = definition.PropertyType.ToTypeId();
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);
        }        
    }
}
