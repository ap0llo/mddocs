using System;
using System.Collections.Generic;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.Utilities.Collections;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Base class for the documentation model of an overload.
    /// </summary>
    public abstract class OverloadDocumentation : IAssemblyMemberDocumentation, IObsoleteableDocumentation
    {
        /// <summary>
        /// Gets the id of the overload.
        /// </summary>
        public MemberId MemberId { get; }

        /// <summary>
        /// Gets the <c>summary</c> documentation for the overload.
        /// </summary>
        public TextBlock Summary { get; }

        /// <summary>
        /// Gets the <c>remarks</c> documentation for the overload.
        /// </summary>
        public TextBlock Remarks { get; }

        /// <summary>
        /// Gets the <c>seealso</c> documentation items for the overload.
        /// </summary>
        public IReadOnlyList<SeeAlsoElement> SeeAlso { get; }

        /// <summary>
        /// Gets the signature of the overload.
        /// </summary>
        public abstract string Signature { get; }

        /// <summary>
        /// Gets the overloads parameters.
        /// </summary>
        public abstract IReadOnlyList<ParameterDocumentation> Parameters { get; }

        /// <summary>
        /// Gets the overloads type parameters.
        /// </summary>
        public abstract IReadOnlyList<TypeParameterDocumentation> TypeParameters { get; }

        /// <summary>
        /// Gets the definition of the overloads as C# code.
        /// </summary>
        public abstract string CSharpDefinition { get; }

        /// <summary>
        /// Gets the overload's return type.
        /// </summary>
        public abstract TypeId Type { get; }

        /// <summary>
        /// Gets the <c>returns</c> documentation for the overload.
        /// </summary>
        public TextBlock Returns { get; }

        /// <summary>
        /// Gets the documented exceptions for the overload.
        /// </summary>
        public IReadOnlyList<ExceptionElement> Exceptions { get; }

        /// <summary>
        /// Gets the <c>example</c> documentation for the overload.
        /// </summary>
        public TextBlock Example { get; }

        /// <inheritdoc />
        public abstract bool IsObsolete { get; }

        /// <inheritdoc />
        public abstract string? ObsoleteMessage { get; }

        /// <inheritdoc />
        public abstract AssemblyDocumentation AssemblyDocumentation { get; }


        // private protected constructor => prevent implementation outside of this assembly
        private protected OverloadDocumentation(MemberId memberId, IXmlDocsProvider xmlDocsProvider)
        {
            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));

            var documentationComments = xmlDocsProvider.TryGetDocumentationComments(memberId);
            Summary = documentationComments?.Summary ?? TextBlock.Empty;
            Remarks = documentationComments?.Remarks ?? TextBlock.Empty;
            SeeAlso = documentationComments?.SeeAlso?.AsReadOnlyList() ?? Array.Empty<SeeAlsoElement>();
            Returns = documentationComments?.Returns ?? TextBlock.Empty;
            Exceptions = documentationComments?.Exceptions?.AsReadOnlyList() ?? Array.Empty<ExceptionElement>();
            Example = documentationComments?.Example ?? TextBlock.Empty;
        }


        /// <inheritdoc />
        public abstract IDocumentation? TryGetDocumentation(MemberId id);
    }
}
