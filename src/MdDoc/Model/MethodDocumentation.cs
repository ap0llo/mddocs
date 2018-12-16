using MdDoc.Model.XmlDocs;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MdDoc.Model
{
    public class MethodDocumentation : MemberDocumentation
    {
        private readonly IXmlDocsProvider m_XmlDocsProvider;

        public string Name { get; }

        public IReadOnlyCollection<MethodOverloadDocumentation> Overloads { get; }


        internal MethodDocumentation(TypeDocumentation typeDocumentation, IEnumerable<MethodDefinition> definitions, IXmlDocsProvider xmlDocsProvider) : base(typeDocumentation)
        {
            if (definitions == null)
                throw new ArgumentNullException(nameof(definitions));

            Overloads = definitions.Select(d => new MethodOverloadDocumentation(this, d, xmlDocsProvider)).ToArray();
            Name = Overloads.Select(x => x.MethodName).Distinct().Single();            
        }
        
    }
}
