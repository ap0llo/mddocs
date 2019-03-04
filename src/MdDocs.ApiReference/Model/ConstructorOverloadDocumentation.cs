using System;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public sealed class ConstructorOverloadDocumentation : MethodLikeOverloadDocumentation
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
