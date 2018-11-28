using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
    public sealed class ParameterDocumentation : IDocumentation
    {
        public string Name => Definition.Name;

        public TypeName Type { get; }

        public OverloadDocumentation OverloadDocumentation { get; }

        public ParameterDefinition Definition { get; }


        public ParameterDocumentation(OverloadDocumentation overloadDocumentation, ParameterDefinition definition)
        {
            OverloadDocumentation = overloadDocumentation ?? throw new ArgumentNullException(nameof(overloadDocumentation));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Type = new TypeName(definition.ParameterType);
        }


        public TypeDocumentation TryGetDocumentation(TypeName type) => 
            OverloadDocumentation.TryGetDocumentation(type);
    }
}
