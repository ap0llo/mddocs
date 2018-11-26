using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace MdDoc.Model
{
    public sealed class MethodOverload
    {
        private readonly MethodFormatter m_MethodFormatter = MethodFormatter.Instance;


        public string MethodName => Definition.Name;

        public MethodDefinition Definition { get; }

        public string Signature => m_MethodFormatter.GetSignature(Definition);

        public IReadOnlyList<ParameterDocumentation> Parameters { get; }


        public MethodOverload(MethodDefinition definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));

            Parameters = definition.HasParameters
                ? Array.Empty<ParameterDocumentation>()
                : definition.Parameters.Select(p => new ParameterDocumentation(p)).ToArray();            
        }
    }
}
