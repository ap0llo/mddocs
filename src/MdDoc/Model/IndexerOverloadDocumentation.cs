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

        public override string CSharpDefinition { get; }


        internal PropertyDefinition Definition { get; }


        internal IndexerOverloadDocumentation(IndexerDocumentation indexerDocumentation, PropertyDefinition definition, IXmlDocsProvider xmlDocsProvider) : base(definition?.ToMemberId(), xmlDocsProvider)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));

            Parameters = definition.HasParameters
                ? Array.Empty<ParameterDocumentation>()
                : definition.Parameters.Select(p => new ParameterDocumentation(this, p)).ToArray();

            Signature = MethodFormatter.Instance.GetSignature(definition);
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);
        }


        public override IDocumentation TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : IndexerDocumentation.TryGetDocumentation(id);
    }
}
