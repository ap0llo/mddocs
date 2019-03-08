using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.Utilities.Collections;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public class MethodDocumentation : OverloadableMemberDocumentation<MethodOverloadDocumentation>
    {
        private readonly IDictionary<MemberId, MethodOverloadDocumentation> m_Overloads;        


        public string Name { get; }

        public override IReadOnlyCollection<MethodOverloadDocumentation> Overloads { get; }


        internal MethodDocumentation(TypeDocumentation typeDocumentation, IEnumerable<MethodDefinition> definitions, IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation)
        {
            if (definitions == null)
                throw new ArgumentNullException(nameof(definitions));

            m_Overloads = definitions
                .Select(d => new MethodOverloadDocumentation(this, d, xmlDocsProvider))
                .ToDictionary(d => d.MemberId);

            Overloads = ReadOnlyCollectionAdapter.Create(m_Overloads.Values);

            Name = Overloads.Select(x => x.MethodName).Distinct().Single();            
        }


        public override IDocumentation TryGetDocumentation(MemberId id)
        {
            if (id is MethodId methodId &&
               methodId.DefiningType.Equals(TypeDocumentation.TypeId) &&
               methodId.Name == Name)
            {
                return m_Overloads.GetValueOrDefault(methodId);
            }
            else
            {
                return TypeDocumentation.TryGetDocumentation(id);
            }
        }
    }
}
