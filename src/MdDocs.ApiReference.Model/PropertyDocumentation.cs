using System;
using System.Collections.Generic;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.Utilities.Collections;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model of a property.
    /// </summary>
    public sealed class PropertyDocumentation : SimpleMemberDocumentation
    {
        /// <inheritdoc />
        public override string Name => Definition.Name;

        /// <inheritdoc />
        public override TypeId Type { get; }

        /// <inheritdoc />
        public override string CSharpDefinition { get; }

        /// <summary>
        /// Gets the <c>value</c> documentation of the property.
        /// </summary>
        public TextBlock Value { get; }

        /// <summary>
        /// Gets the documented exceptions for the property.
        /// </summary>
        public IReadOnlyList<ExceptionElement> Exceptions { get; }

        /// <summary>
        /// Gets the underlying Mono.Cecil definition of the property.
        /// </summary>
        internal PropertyDefinition Definition { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="PropertyDocumentation"/>.
        /// </summary>
        /// <param name="typeDocumentation">The documentation model of the type defining the property.</param>
        /// <param name="definition">The underlying Mono.Cecil definition of the property.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        internal PropertyDocumentation(TypeDocumentation typeDocumentation, PropertyDefinition definition, IXmlDocsProvider xmlDocsProvider)
            : base(typeDocumentation, definition?.ToMemberId(), xmlDocsProvider, definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            xmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));

            Type = definition.PropertyType.ToTypeId();
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);

            var documentationComments = xmlDocsProvider?.TryGetDocumentationComments(MemberId);
            Value = documentationComments?.Value;
            Exceptions = documentationComments?.Exceptions?.AsReadOnlyList() ?? Array.Empty<ExceptionElement>();
        }
    }
}
