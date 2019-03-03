using System;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public class FieldDocumentation : SimpleMemberDocumentation
    {
        public override string Name => Definition.Name;

        public override string CSharpDefinition { get; }

        public override TypeId Type { get; }

        public TextBlock Value { get; }

        internal FieldDefinition Definition { get; }


        internal FieldDocumentation(TypeDocumentation typeDocumentation, FieldDefinition definition, IXmlDocsProvider xmlDocsProvider)
            : base(typeDocumentation, definition?.ToMemberId(), xmlDocsProvider, definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            xmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));

            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);
            Type = definition.FieldType.ToTypeId();

            Value = xmlDocsProvider.TryGetDocumentationComments(MemberId)?.Value;
        }
    }
}
