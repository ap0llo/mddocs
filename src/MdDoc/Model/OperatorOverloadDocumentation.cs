using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace MdDoc.Model
{
    public sealed class OperatorOverloadDocumentation : OverloadDocumentation
    {
        public OperatorKind OperatorKind { get; }
     

        public OperatorOverloadDocumentation(MethodDefinition definition) : base(definition)
        {
            OperatorKind = definition.GetOperatorKind() ?? throw new ArgumentException($"Method {definition.Name} is not an operator overload");
        }
    }
}
