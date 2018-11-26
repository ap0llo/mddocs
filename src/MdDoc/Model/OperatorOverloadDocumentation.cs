using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace MdDoc.Model
{
    public sealed class OperatorOverloadDocumentation
    {
        private readonly MethodFormatter m_MethodFormatter = MethodFormatter.Instance;


        public OperatorKind OperatorKind { get; }

        public MethodDefinition Definition { get; }

        public string Signature => m_MethodFormatter.GetSignature(Definition);

        public IReadOnlyList<ParameterDocumentation> Parameters { get; }


        public OperatorOverloadDocumentation(MethodDefinition definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            OperatorKind = definition.GetOperatorKind() ?? throw new ArgumentException($"Method {definition.Name} is not an operator overload");

            Parameters = definition.HasParameters
                ? Array.Empty<ParameterDocumentation>()
                : definition.Parameters.Select(p => new ParameterDocumentation(p)).ToArray();
        }
    }
}
