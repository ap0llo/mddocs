using System;
using System.Collections.Generic;
using System.Linq;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public abstract class MethodLikeOverloadDocumentation : OverloadDocumentation
    {
        public override string Signature { get; }

        public override IReadOnlyList<ParameterDocumentation> Parameters { get; }

        public override string CSharpDefinition { get; }
        

        internal MethodDefinition Definition { get; }


        internal MethodLikeOverloadDocumentation(
            MethodDefinition definition,
            IXmlDocsProvider xmlDocsProvider) : base(definition?.ToMemberId(), xmlDocsProvider)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));            
            
            Parameters = definition.HasParameters
                ? definition.Parameters.Select(p => new ParameterDocumentation(this, p)).ToArray()
                : Array.Empty<ParameterDocumentation>();

            Signature = MethodFormatter.Instance.GetSignature(Definition);

            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);            
        }        
    }
}
