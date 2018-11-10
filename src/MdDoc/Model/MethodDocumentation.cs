using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Model
{
    public class MethodDocumentation
    {
        private readonly DocumentationContext m_Context;


        public string Name { get; }

        public IReadOnlyCollection<MethodDefinition> Overloads { get; }


        public MethodDocumentation(DocumentationContext context, IEnumerable<MethodDefinition> overloads)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            Overloads = overloads?.ToList() ?? throw new ArgumentNullException(nameof(overloads));
            
            Name = overloads.Select(x => x.Name).Distinct().Single();
        }
        
    }
}
