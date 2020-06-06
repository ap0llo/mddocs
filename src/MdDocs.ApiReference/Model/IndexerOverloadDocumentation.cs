using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.Common;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model for an indexer overload.
    /// </summary>
    public sealed class IndexerOverloadDocumentation : OverloadDocumentation
    {
        /// <summary>
        /// Gets the documentation model of the indexer being overloaded.
        /// </summary>
        public IndexerDocumentation IndexerDocumentation { get; }

        /// <summary>
        /// Gets the signature of the indexer.
        /// </summary>
        public override string Signature { get; }

        /// <summary>
        /// Gets the documentation models for the indexer's parameters.
        /// </summary>
        public override IReadOnlyList<ParameterDocumentation> Parameters { get; }

        /// <summary>
        /// Gets the indexer's type parameters.
        /// </summary>
        /// <remarks>
        /// As C# does not allow definition of indexers with type parameters, this will always be an empty list.
        /// </remarks>
        public override IReadOnlyList<TypeParameterDocumentation> TypeParameters { get; } = Array.Empty<TypeParameterDocumentation>();

        /// <summary>
        /// Gets the definition of the indexer as C# code.
        /// </summary>
        public override string CSharpDefinition { get; }

        /// <summary>
        /// Gets the <c>value</c> documentation for the field.
        /// </summary>
        public TextBlock? Value { get; }

        /// <inheritdoc />
        public override TypeId Type { get; }

        /// <inheritdoc />
        public override bool IsObsolete { get; }

        /// <inheritdoc />
        public override string? ObsoleteMessage { get; }

        /// <summary>
        /// Gets the underlying Mono.Cecil definition of the indexer.
        /// </summary>
        internal PropertyDefinition Definition { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="IndexerOverloadDocumentation"/>
        /// </summary>
        /// <param name="indexerDocumentation">The documentation model of the indexer being overloaded.</param>
        /// <param name="definition">The underlying Mono.Cecil definition of the indexer.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        internal IndexerOverloadDocumentation(
            IndexerDocumentation indexerDocumentation,
            PropertyDefinition definition,
            IXmlDocsProvider xmlDocsProvider) : base(definition.ToMemberId(), xmlDocsProvider)
        {
            IndexerDocumentation = indexerDocumentation ?? throw new ArgumentNullException(nameof(indexerDocumentation));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            xmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));

            Parameters = definition.HasParameters
                ? definition.Parameters.Select(p => new ParameterDocumentation(this, p, xmlDocsProvider)).ToArray()
                : Array.Empty<ParameterDocumentation>();

            Type = definition.PropertyType.ToTypeId();
            Signature = SignatureFormatter.GetSignature(definition);
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);
            Value = xmlDocsProvider.TryGetDocumentationComments(MemberId)?.Value;

            IsObsolete = definition.IsObsolete(out var obsoleteMessage);
            ObsoleteMessage = obsoleteMessage;
        }


        /// <inheritdoc />
        public override IDocumentation? TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : IndexerDocumentation.TryGetDocumentation(id);
    }
}
