using System;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public class PropertyDocumentation : SimpleMemberDocumentation
    {        
        public override string Name => Definition.Name;

        public override TypeId Type { get; }

        public TextBlock Value { get; }

        public override string CSharpDefinition { get; }
        
        internal PropertyDefinition Definition { get; }


        internal PropertyDocumentation(
            TypeDocumentation typeDocumentation,
            PropertyDefinition definition,
            IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation, definition?.ToMemberId(), xmlDocsProvider)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            xmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));

            Type = definition.PropertyType.ToTypeId();
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);

            Value = xmlDocsProvider?.TryGetDocumentationComments(MemberId)?.Value;
        }        
    }
}
