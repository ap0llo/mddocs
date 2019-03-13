using System;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.Utilities.Collections;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model for a type's or method's generic type parameter.
    /// </summary>
    public sealed class TypeParameterDocumentation : IDocumentation
    {
        private readonly IDocumentation m_Parent;


        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string Name => Definition.Name;

        /// <summary>
        /// Gets the type parameter's description 
        /// </summary>
        /// <value>The documentation as provided by the user or <c>null</c> if no description was specified.</value>
        public TextBlock Description { get; }


        /// <summary>
        /// Gets the underlying Mono.Cecil definition of the type parameter.
        /// </summary>
        internal GenericParameter Definition { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="TypeParameterDocumentation"/>.
        /// </summary>
        /// <param name="parent">The parent documentation model (the type of method that defines the type parameter).</param>
        /// <param name="parentId">The if of the parent documentation model (the type of method that defines the type parameter).</param>
        /// <param name="definition">The underlying Mono.Cecil definition of the type parameter.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        internal TypeParameterDocumentation(IDocumentation parent, MemberId parentId, GenericParameter definition, IXmlDocsProvider xmlDocsProvider)
        {
            m_Parent = parent;
            parentId = parentId ?? throw new ArgumentNullException(nameof(parentId));
            Definition = definition;
            xmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));

            Description = xmlDocsProvider.TryGetDocumentationComments(parentId)?.TypeParameters?.GetValueOrDefault(definition.Name);
        }


        /// <inheritdoc />
        public IDocumentation TryGetDocumentation(MemberId member) => m_Parent.TryGetDocumentation(member);
    }
}
