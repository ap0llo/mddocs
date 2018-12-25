using System;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public sealed class MethodOverloadDocumentation : OverloadDocumentation
    {
        public string MethodName => Definition.Name;

        public MethodDocumentation MethodDocumentation { get; }

        
        internal MethodOverloadDocumentation(MethodDocumentation methodDocumentation, MethodDefinition definition, IXmlDocsProvider xmlDocsProvider) : base(definition, xmlDocsProvider)
        {
            MethodDocumentation = methodDocumentation ?? throw new ArgumentNullException(nameof(methodDocumentation));            
            xmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));
        }


        public override IDocumentation TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : MethodDocumentation.TryGetDocumentation(id);
    }
}
