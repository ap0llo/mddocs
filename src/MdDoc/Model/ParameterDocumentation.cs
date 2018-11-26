using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
    public class ParameterDocumentation
    {
        public string Name => Definition.Name;

        public TypeReference Type => Definition.ParameterType;

        public ParameterDefinition Definition { get; }


        public ParameterDocumentation(ParameterDefinition definition)
        {
            Definition = definition;
        }
        
    }
}
