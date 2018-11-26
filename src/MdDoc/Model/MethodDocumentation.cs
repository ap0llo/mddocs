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

        public IReadOnlyCollection<MethodOverloadDocumentation> Overloads { get; }


        public MethodDocumentation(TypeDocumentation typeDocumentation, IEnumerable<MethodDefinition> definitions) : base(typeDocumentation)
        {
            if (definitions == null)
                throw new ArgumentNullException(nameof(definitions));

            Overloads = definitions.Select(d => new MethodOverloadDocumentation(this, d)).ToArray();
            Name = Overloads.Select(x => x.MethodName).Distinct().Single();
        }
        
    }
}
