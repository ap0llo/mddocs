using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace MdDoc.Model
{
    public abstract class OverloadDocumentation : IDocumentation
    {
        private readonly MethodFormatter m_MethodFormatter = MethodFormatter.Instance;


        public MemberId MemberId { get; }

        //TODO: generate signature for suited for operator overloads, e.g. the operator name not the underlying method name
        public string Signature => m_MethodFormatter.GetSignature(Definition);

        public IReadOnlyList<ParameterDocumentation> Parameters { get; }

        internal MethodDefinition Definition { get; }


        public OverloadDocumentation(MethodDefinition definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            MemberId = definition.ToMemberId();

            Parameters = definition.HasParameters
                ? Array.Empty<ParameterDocumentation>()
                : definition.Parameters.Select(p => new ParameterDocumentation(this, p)).ToArray();
        }


        public abstract IDocumentation TryGetDocumentation(MemberId id);
    }
}
