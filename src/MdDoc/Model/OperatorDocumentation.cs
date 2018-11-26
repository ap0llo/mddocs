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

        public IReadOnlyCollection<MethodOverload> Overloads { get; }


        public OperatorDocumentation(TypeDocumentation typeDocumentation, IEnumerable<MethodOverload> overloads) : base(typeDocumentation)
        {
            Overloads = overloads?.ToArray() ?? throw new ArgumentNullException(nameof(overloads));

            var definitionList = new List<MethodDefinition>();

            OperatorKind? previousKind = null;
            foreach (var overload in overloads)
            {
                var kind = overload.Definition.GetOperatorKind() ?? throw new ArgumentException($"Method '{overload.MethodName}' is not a operator overload");
                definitionList.Add(overload.Definition);

                if(previousKind.HasValue && previousKind.Value != kind)
                {
                    throw new ArgumentException("Cannot combine overloads of different operators");
                }
                previousKind = kind;
                Kind = kind;
            }            
        }
        
    }
}
