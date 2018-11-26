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

        public IReadOnlyCollection<MethodOverload> Overloads { get; }


        public MethodDocumentation(TypeDocumentation typeDocumentation, IEnumerable<MethodOverload> overloads) : base(typeDocumentation)
        {
            Overloads = overloads?.ToArray() ?? throw new ArgumentNullException(nameof(overloads));
            Name = overloads.Select(x => x.MethodName).Distinct().Single();
        }
        
    }
}
