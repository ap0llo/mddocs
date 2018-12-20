using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MdDoc.Model
{
    public class OperatorDocumentation : MemberDocumentation
    {
        public OperatorKind Kind { get; }

        public IReadOnlyCollection<OperatorOverloadDocumentation> Overloads { get; }       

        public OperatorDocumentation(TypeDocumentation typeDocumentation, IEnumerable<MethodDefinition> definitions) : base(typeDocumentation)
        {        
            if (definitions == null)
                throw new ArgumentNullException(nameof(definitions));

            Overloads = definitions.Select(d => new OperatorOverloadDocumentation(this, d)).ToArray();
           
            OperatorKind? previousKind = null;
            foreach (var overload in Overloads)
            {                
                if(previousKind.HasValue && previousKind.Value != overload.OperatorKind)
                {
                    throw new ArgumentException("Cannot combine overloads of different operators");
                }
                previousKind = overload.OperatorKind;
                Kind = overload.OperatorKind;
            }            
        }
        
    }
}
