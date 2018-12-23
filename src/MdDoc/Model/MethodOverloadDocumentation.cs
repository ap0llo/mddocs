using System;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public sealed class MethodOverloadDocumentation : OverloadDocumentation
    {
        public string MethodName => Definition.Name;

        public MethodDocumentation MethodDocumentation { get; }

        public TextBlock Summary { get; }

        public string CSharpDefinition { get; }


        internal MethodOverloadDocumentation(MethodDocumentation methodDocumentation, MethodDefinition definition, IXmlDocsProvider xmlDocsProvider) : base(definition)
        {
            MethodDocumentation = methodDocumentation ?? throw new ArgumentNullException(nameof(methodDocumentation));            
            xmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));
            Summary = xmlDocsProvider.TryGetDocumentationComments(MemberId)?.Summary;
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);
        }


        public override IDocumentation TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : MethodDocumentation.TryGetDocumentation(id);
    }
}
