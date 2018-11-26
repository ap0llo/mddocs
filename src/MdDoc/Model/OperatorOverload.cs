using System;
using Mono.Cecil;

namespace MdDoc.Model
{
    public sealed class OperatorOverload
    {
        private readonly MethodFormatter m_MethodFormatter = MethodFormatter.Instance;


        public OperatorKind OperatorKind { get; }

        public MethodDefinition Definition { get; }

        public string Signature => m_MethodFormatter.GetSignature(Definition);


        public OperatorOverload(MethodDefinition definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            OperatorKind = definition.GetOperatorKind() ?? throw new ArgumentException($"Method {definition.Name} is not an operator overload");
        }
    }
}
