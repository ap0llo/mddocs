using System;
using System.Collections.Generic;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public class PropertyDocumentation : SimpleMemberDocumentation
    {
        public override string Name => Definition.Name;

        public override TypeId Type { get; }

        public TextBlock Value { get; }

        public override string CSharpDefinition { get; }

        public IReadOnlyList<ExceptionElement> Exceptions { get; }


        internal PropertyDefinition Definition { get; }


        internal PropertyDocumentation(TypeDocumentation typeDocumentation, PropertyDefinition definition, IXmlDocsProvider xmlDocsProvider)
            : base(typeDocumentation, definition?.ToMemberId(), xmlDocsProvider, definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            xmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));

            Type = definition.PropertyType.ToTypeId();
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);

            var documentationComments = xmlDocsProvider?.TryGetDocumentationComments(MemberId);
            Value = documentationComments?.Value;
            Exceptions = documentationComments?.Exceptions?.ToReadOnly() ?? Array.Empty<ExceptionElement>();
        }
    }
}
