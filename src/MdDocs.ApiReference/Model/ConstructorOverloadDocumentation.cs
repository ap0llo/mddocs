using System;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model of a constructor overload.
    /// </summary>
    public sealed class ConstructorOverloadDocumentation : MethodLikeOverloadDocumentation
    {
        /// <summary>
        /// Gets the documentation model of the constructor being overloaded by this overload.
        /// </summary>
        public ConstructorDocumentation ConstructorDocumentation { get; }

        /// <inheritdoc />
        public override AssemblyDocumentation AssemblyDocumentation =>
            ConstructorDocumentation.AssemblyDocumentation;


        /// <summary>
        /// Initializes a new instance of <see cref="ConstructorOverloadDocumentation"/>
        /// </summary>
        /// <param name="methodDocumentation">The documentation model of the constructor being overloaded.</param>
        /// <param name="definition">The definition of the constructor method.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        /// <exception cref="ArgumentNullException">Thrown when one of the constructor arguments is null.</exception>
        internal ConstructorOverloadDocumentation(ConstructorDocumentation methodDocumentation, MethodDefinition definition, IXmlDocsProvider xmlDocsProvider) : base(definition, xmlDocsProvider)
        {
            ConstructorDocumentation = methodDocumentation ?? throw new ArgumentNullException(nameof(methodDocumentation));
        }


        /// <inheritdoc />
        public override IDocumentation? TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : ConstructorDocumentation.TryGetDocumentation(id);
    }
}
