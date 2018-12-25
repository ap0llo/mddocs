using System;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public class FieldDocumentation : SimpleMemberDocumentation
    {
        public override string Name => Definition.Name;
        
        public override string CSharpDefinition { get; }

        internal FieldDefinition Definition { get; }
        

        internal FieldDocumentation(
            TypeDocumentation typeDocumentation,
            FieldDefinition definition,
            IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation, definition?.ToMemberId(), xmlDocsProvider)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);
        }


    }
}
