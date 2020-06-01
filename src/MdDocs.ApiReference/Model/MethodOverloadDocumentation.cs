using System;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model of a method overload.
    /// </summary>
    public sealed class MethodOverloadDocumentation : MethodLikeOverloadDocumentation
    {
        /// <summary>
        /// Gets the documentation model of the method being overloaded.
        /// </summary>
        public MethodDocumentation MethodDocumentation { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="MethodOverloadDocumentation"/>.
        /// </summary>
        /// <param name="methodDocumentation">The documentation model of the method being overloaded.</param>
        /// <param name="definition">The underlying Mono.Cecil definition of the method.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        internal MethodOverloadDocumentation(MethodDocumentation methodDocumentation, MethodDefinition definition, IXmlDocsProvider xmlDocsProvider)
            : base(definition, xmlDocsProvider)
        {
            MethodDocumentation = methodDocumentation ?? throw new ArgumentNullException(nameof(methodDocumentation));
        }


        /// <inheritdoc />
        public override IDocumentation? TryGetDocumentation(MemberId id) =>
            MemberId.Equals(id) ? this : MethodDocumentation.TryGetDocumentation(id);
    }
}
