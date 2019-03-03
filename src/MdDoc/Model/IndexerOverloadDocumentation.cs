using System;
using System.Collections.Generic;
using System.Linq;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public sealed class IndexerOverloadDocumentation : OverloadDocumentation
    {
        public string Name => Definition.Name;

        public IndexerDocumentation IndexerDocumentation { get; }

        public override string Signature { get; }

        public override IReadOnlyList<ParameterDocumentation> Parameters { get; }

        //indexers cannot have type parameters
        public override IReadOnlyList<TypeParameterDocumentation> TypeParameters { get; } = Array.Empty<TypeParameterDocumentation>();

        public override string CSharpDefinition { get; }

        public TextBlock Value { get; }

        public override TypeId Type { get; }

        public override bool IsObsolete { get; }

        public override string ObsoleteMessage { get; }

        internal PropertyDefinition Definition { get; }


        internal IndexerOverloadDocumentation(
            IndexerDocumentation indexerDocumentation,
            PropertyDefinition definition,
            IXmlDocsProvider xmlDocsProvider) : base(definition?.ToMemberId(), xmlDocsProvider)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            xmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));

            Parameters = definition.HasParameters
                ? definition.Parameters.Select(p => new ParameterDocumentation(this, p, xmlDocsProvider)).ToArray()
                : Array.Empty<ParameterDocumentation>();


            Signature = MethodFormatter.Instance.GetSignature(definition);
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);
            Value = xmlDocsProvider.TryGetDocumentationComments(MemberId)?.Value;
            Type = definition.PropertyType.ToTypeId();

            IsObsolete = definition.IsObsolete(out var obsoleteMessage);
            ObsoleteMessage = obsoleteMessage;
        }


        public override IDocumentation TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : IndexerDocumentation.TryGetDocumentation(id);
    }
}
