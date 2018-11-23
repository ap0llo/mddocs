using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Model
{
    public class MethodDocumentation : MemberDocumentation
    {
        private readonly DocumentationContext m_Context;


        
        public string Name { get; }

        public IReadOnlyCollection<MethodDefinition> Definitions { get; }


        public MethodDocumentation(TypeDocumentation typeDocumentation, DocumentationContext context, IEnumerable<MethodDefinition> definitions) : base(typeDocumentation)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            Definitions = definitions?.ToList() ?? throw new ArgumentNullException(nameof(definitions));
            
            Name = definitions.Select(x => x.Name).Distinct().Single();
        }
        
    }
}
