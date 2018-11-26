using System;
using Mono.Cecil;

namespace MdDoc.Model
{
    public sealed class MethodOverload
    {
        public string MethodName => Definition.Name;

        public MethodDefinition Definition { get; }


        public MethodOverload(MethodDefinition definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }
    }
}
