using System;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public sealed class ConstructorOverloadDocumentation : OverloadDocumentation
    {
        public string MethodName => Definition.Name;

        public ConstructorDocumentation ConstructorDocumentation { get; }

        public TextBlock Summary { get; }

        public string CSharpDefinition { get; }


        internal ConstructorOverloadDocumentation(ConstructorDocumentation methodDocumentation, MethodDefinition definition, IXmlDocsProvider xmlDocsProvider) : base(definition)
        {
            ConstructorDocumentation = methodDocumentation ?? throw new ArgumentNullException(nameof(methodDocumentation));            
            xmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));
            Summary = xmlDocsProvider.TryGetDocumentationComments(MemberId)?.Summary;
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);
        }


        public override IDocumentation TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : ConstructorDocumentation.TryGetDocumentation(id);
    }
}
