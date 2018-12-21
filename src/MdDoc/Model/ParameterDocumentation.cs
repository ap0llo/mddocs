using Mono.Cecil;
using System;

namespace MdDoc.Model
{
    public sealed class ParameterDocumentation : IDocumentation
    {
        public string Name => Definition.Name;

        public TypeId ParameterType { get; }

        public OverloadDocumentation OverloadDocumentation { get; }

        internal ParameterDefinition Definition { get; }


        public ParameterDocumentation(OverloadDocumentation overloadDocumentation, ParameterDefinition definition)
        {
            OverloadDocumentation = overloadDocumentation ?? throw new ArgumentNullException(nameof(overloadDocumentation));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            ParameterType = definition.ParameterType.ToTypeId();
        }


        public IDocumentation TryGetDocumentation(MemberId id) => 
            OverloadDocumentation.TryGetDocumentation(id);



    }
}
