using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.Utilities.Collections;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public class OperatorDocumentation : MemberDocumentation
    {
        private readonly IDictionary<MemberId, OperatorOverloadDocumentation> m_Overloads;


        public OperatorKind Kind { get; }

        public IReadOnlyCollection<OperatorOverloadDocumentation> Overloads { get; }       


        internal OperatorDocumentation(TypeDocumentation typeDocumentation, IEnumerable<MethodDefinition> definitions, IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation)
        {        
            if (definitions == null)
                throw new ArgumentNullException(nameof(definitions));

            m_Overloads = definitions
                .Select(d => new OperatorOverloadDocumentation(this, d, xmlDocsProvider))
                .ToDictionary(x => x.MemberId);

            Overloads = ReadOnlyCollectionAdapter.Create(m_Overloads.Values);

            var operatorKinds = Overloads.Select(x => x.OperatorKind).Distinct().ToArray();

            Kind = operatorKinds.Length == 1
                ? operatorKinds[0]
                : throw new ArgumentException("Cannot combine overloads of different operators");
        }


        public override IDocumentation TryGetDocumentation(MemberId id)
        {
            if(id is MethodId methodId && 
               methodId.DefiningType.Equals(TypeDocumentation.TypeId) &&
               methodId.GetOperatorKind() == Kind)
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
