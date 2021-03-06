﻿using System;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public sealed class OperatorOverloadDocumentation : MethodLikeOverloadDocumentation
    {
        public OperatorKind OperatorKind { get; }

        public OperatorDocumentation OperatorDocumentation { get; }

        /// <inheritdoc />
        public override AssemblyDocumentation AssemblyDocumentation =>
            OperatorDocumentation.AssemblyDocumentation;


        internal OperatorOverloadDocumentation(OperatorDocumentation operatorDocumentation, MethodDefinition definition, IXmlDocsProvider xmlDocsProvider) : base(definition, xmlDocsProvider)
        {
            OperatorKind = definition.GetOperatorKind() ?? throw new ArgumentException($"Method {definition.Name} is not an operator overload");
            OperatorDocumentation = operatorDocumentation ?? throw new ArgumentNullException(nameof(operatorDocumentation));
        }


        /// <inheritdoc />
        public override IDocumentation? TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : OperatorDocumentation.TryGetDocumentation(id);
    }
}
