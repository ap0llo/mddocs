using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.Utilities.Collections;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model for indexers.
    /// </summary>
    public sealed class IndexerDocumentation : OverloadableMemberDocumentation<IndexerOverloadDocumentation>
    {
        private readonly IDictionary<MemberId, IndexerOverloadDocumentation> m_Overloads;


        /// <summary>
        /// Gets the name of the indexer. Usually the name is <c>Item</c>.
        /// </summary>
        public string Name { get; }

        /// <inheritDoc />
        public override IReadOnlyCollection<IndexerOverloadDocumentation> Overloads { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="IndexerDocumentation"/>.
        /// </summary>
        /// <param name="typeDocumentation">The documentation model of the type defining the indexer.</param>
        /// <param name="definitions">The Mono.Cecil definitions of the all the indexer's overloads.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        internal IndexerDocumentation(TypeDocumentation typeDocumentation, IEnumerable<PropertyDefinition> definitions, IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation)
        {
            if (definitions == null)
                throw new ArgumentNullException(nameof(definitions));

            if (definitions.Select(x => x.Name).Distinct().Count() > 1)
                throw new ArgumentException("All definitions have to be overloads of the same indexer", nameof(definitions));

            m_Overloads = definitions
                .Select(d => new IndexerOverloadDocumentation(this, d, xmlDocsProvider))
                .ToDictionary(d => d.MemberId);

            Overloads = ReadOnlyCollectionAdapter.Create(m_Overloads.Values);

            Name = definitions.First().Name;
        }


        /// <inheritdoc />
        public override IDocumentation TryGetDocumentation(MemberId id)
        {
            if (id is PropertyId propertyId &&
               propertyId.DefiningType.Equals(TypeDocumentation.TypeId) &&
               propertyId.Name == Name)
            {
                return m_Overloads.GetValueOrDefault(propertyId);
            }
            else
            {
                return TypeDocumentation.TryGetDocumentation(id);
            }
        }
    }
}
