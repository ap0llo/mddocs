using System;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public class PropertyDocumentation : SimpleMemberDocumentation
    {        
        public override string Name => Definition.Name;

        public TypeId PropertyType { get; }

        // Indexeres are modeled as properties with parameters
        public bool IsIndexer => Definition.HasParameters;
        
        public override string CSharpDefinition { get; }       

        internal PropertyDefinition Definition { get; }


        internal PropertyDocumentation(
            TypeDocumentation typeDocumentation,
            PropertyDefinition definition,
            IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation, definition?.ToMemberId(), xmlDocsProvider)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            PropertyType = definition.PropertyType.ToTypeId();
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);
        }        
    }
}
