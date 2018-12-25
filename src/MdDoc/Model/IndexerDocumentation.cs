using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.Utilities.Collections;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public class IndexerDocumentation : OverloadableMemberDocumentation<IndexerOverloadDocumentation>
    {       
        private readonly IDictionary<MemberId, IndexerOverloadDocumentation> m_Overloads;


        public string Name { get; }

        public override IReadOnlyCollection<IndexerOverloadDocumentation> Overloads { get; }


        internal IndexerDocumentation(TypeDocumentation typeDocumentation, IEnumerable<PropertyDefinition> definitions, IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation)
        {
            if (definitions == null)
                throw new ArgumentNullException(nameof(definitions));

            m_Overloads = definitions
                .Select(d => new IndexerOverloadDocumentation(this, d, xmlDocsProvider))
                .ToDictionary(d => d.MemberId);

            Overloads = ReadOnlyCollectionAdapter.Create(m_Overloads.Values);

            Name = Overloads.Select(x => x.Name).Distinct().Single();
        }

        

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
