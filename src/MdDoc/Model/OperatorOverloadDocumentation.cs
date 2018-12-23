using System;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public sealed class OperatorOverloadDocumentation : OverloadDocumentation
    {
        public OperatorKind OperatorKind { get; }

        public OperatorDocumentation OperatorDocumentation { get; }

        public TextBlock Summary { get; }

        public string CSharpDefinition { get; }


        internal OperatorOverloadDocumentation(OperatorDocumentation operatorDocumentation, MethodDefinition definition, IXmlDocsProvider xmlDocsProvider) : base(definition)
        {
            OperatorKind = definition.GetOperatorKind() ?? throw new ArgumentException($"Method {definition.Name} is not an operator overload");
            OperatorDocumentation = operatorDocumentation ?? throw new ArgumentNullException(nameof(operatorDocumentation));
            Summary = xmlDocsProvider.TryGetDocumentationComments(MemberId)?.Summary;
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);
        }


        public override IDocumentation TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : OperatorDocumentation.TryGetDocumentation(id);
    }
}
