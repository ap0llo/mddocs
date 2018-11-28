using System;
using Mono.Cecil;

namespace MdDoc.Model
{
    public sealed class MethodOverloadDocumentation : OverloadDocumentation
    {
        public string MethodName => Definition.Name;

        public MethodDocumentation MethodDocumentation { get; }


        public MethodOverloadDocumentation(MethodDocumentation methodDocumentation, MethodDefinition definition) : base(definition)
        {
            MethodDocumentation = methodDocumentation ?? throw new ArgumentNullException(nameof(methodDocumentation));
        }


        public override TypeDocumentation TryGetDocumentation(TypeName type) => 
            MethodDocumentation.TryGetDocumentation(type);
    }
}
