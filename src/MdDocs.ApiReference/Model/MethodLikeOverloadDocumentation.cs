using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.Common;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Base class for overloads of type members that are method-like (as opposed to indexers).
    /// </summary>
    public abstract class MethodLikeOverloadDocumentation : OverloadDocumentation
    {
        private readonly IXmlDocsProvider m_XmlDocsProvider;


        /// <inheritdoc />
        public override string Signature { get; }

        /// <inheritdoc />
        public override IReadOnlyList<ParameterDocumentation> Parameters { get; }

        /// <inheritdoc />
        public override string CSharpDefinition { get; }

        /// <inheritdoc />
        public override IReadOnlyList<TypeParameterDocumentation> TypeParameters { get; }

        /// <inheritdoc />
        public override TypeId Type { get; }

        /// <inheritdoc />
        public override bool IsObsolete { get; }

        /// <inheritdoc />
        public override string? ObsoleteMessage { get; }

        /// <summary>
        /// Gets the underlying Mono.Cecil definition of the method overload.
        /// </summary>
        internal MethodDefinition Definition { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="MethodLikeOverloadDocumentation"/>.
        /// </summary>
        /// <param name="definition">The underlying Mono.Cecil definition of the method overload.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        // private protected constructor => prevent implementation outside of this assembly
        private protected MethodLikeOverloadDocumentation(MethodDefinition definition, IXmlDocsProvider xmlDocsProvider)
            : base(definition.ToMemberId(), xmlDocsProvider)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            m_XmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));

            Parameters = definition.HasParameters
                ? definition.Parameters.Select(p => new ParameterDocumentation(this, p, xmlDocsProvider)).ToArray()
                : Array.Empty<ParameterDocumentation>();

            Type = definition.ReturnType.ToTypeId();
            TypeParameters = LoadTypeParameters();
            Signature = SignatureFormatter.GetSignature(Definition);
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);

            IsObsolete = Definition.IsObsolete(out var obsoleteMessage);
            ObsoleteMessage = obsoleteMessage;
        }


        private IReadOnlyList<TypeParameterDocumentation> LoadTypeParameters()
        {
            if (!Definition.HasGenericParameters)
                return Array.Empty<TypeParameterDocumentation>();
            else
                return Definition.GenericParameters
                    .Select(p => new TypeParameterDocumentation(this, MemberId, p, m_XmlDocsProvider))
                    .ToArray();
        }
    }
}
