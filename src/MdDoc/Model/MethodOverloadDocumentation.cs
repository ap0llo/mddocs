using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace MdDoc.Model
{
    public sealed class MethodOverloadDocumentation : OverloadDocumentation
    {
        public string MethodName => Definition.Name;


        public MethodOverloadDocumentation(MethodDefinition definition) : base(definition)
        { }        
    }
}
