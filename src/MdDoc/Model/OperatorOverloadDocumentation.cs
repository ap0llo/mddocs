using System;
using Mono.Cecil;

namespace MdDoc.Model
{
    public sealed class OperatorOverloadDocumentation : OverloadDocumentation
    {
        public OperatorKind OperatorKind { get; }

        public OperatorDocumentation OperatorDocumentation { get; }


        public OperatorOverloadDocumentation(OperatorDocumentation operatorDocumentation, MethodDefinition definition) : base(definition)
        {
            OperatorKind = definition.GetOperatorKind() ?? throw new ArgumentException($"Method {definition.Name} is not an operator overload");
            OperatorDocumentation = operatorDocumentation ?? throw new ArgumentNullException(nameof(operatorDocumentation));
        }


        public override TypeDocumentation TryGetDocumentation(TypeName type) => 
            OperatorDocumentation.TryGetDocumentation(type);
    }
}
