using System;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public sealed class ConstructorOverloadDocumentation : OverloadDocumentation
    {
        public string MethodName => Definition.Name;

        public ConstructorDocumentation ConstructorDocumentation { get; }


        internal ConstructorOverloadDocumentation(ConstructorDocumentation methodDocumentation, MethodDefinition definition, IXmlDocsProvider xmlDocsProvider) : base(definition, xmlDocsProvider)
        {
            ConstructorDocumentation = methodDocumentation ?? throw new ArgumentNullException(nameof(methodDocumentation));            
            xmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));            
        }


        public override IDocumentation TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : ConstructorDocumentation.TryGetDocumentation(id);
    }
}
