using System;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public class EventDocumentation : SimpleMemberDocumentation
    {
        public override string Name => Definition.Name;
        
        public override string CSharpDefinition { get; }

        internal EventDefinition Definition { get; }


        internal EventDocumentation(
            TypeDocumentation typeDocumentation,
            EventDefinition definition,
            IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation, definition?.ToMemberId(), xmlDocsProvider)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));            
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);
        }     
    }
}
