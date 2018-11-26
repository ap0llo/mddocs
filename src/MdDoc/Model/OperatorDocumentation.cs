using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Model
{
    public class OperatorDocumentation : MemberDocumentation
    {
        public OperatorKind Kind { get; }

        public IReadOnlyCollection<OperatorOverloadDocumentation> Overloads { get; }


        public OperatorDocumentation(TypeDocumentation typeDocumentation, IEnumerable<OperatorOverloadDocumentation> overloads) : base(typeDocumentation)
        {
            Overloads = overloads?.ToArray() ?? throw new ArgumentNullException(nameof(overloads));
           
            OperatorKind? previousKind = null;
            foreach (var overload in overloads)
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
