using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Model
{
    public class MethodDocumentation : MemberDocumentation
    {
        public string Name { get; }

        public IReadOnlyCollection<MethodDefinition> Definitions { get; }


        public MethodDocumentation(TypeDocumentation typeDocumentation, IEnumerable<MethodDefinition> definitions) : base(typeDocumentation)
        {
            Definitions = definitions?.ToList() ?? throw new ArgumentNullException(nameof(definitions));
            
            Name = definitions.Select(x => x.Name).Distinct().Single();
        }
        
    }
}
