using System;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model for a field.
    /// </summary>
    public sealed class FieldDocumentation : SimpleMemberDocumentation
    {
        /// <inheritdoc />
        public override string Name => Definition.Name;

        /// <inheritdoc />
        public override string CSharpDefinition { get; }

        /// <inheritdoc />
        public override TypeId Type { get; }

        /// <summary>
        /// Gets the <c>value</c> documentation for the field.
        /// </summary>
        public TextBlock Value { get; }

        /// <summary>
        /// Gets the underlying Mono.Cecil definition of the field.
        /// </summary>
        internal FieldDefinition Definition { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="FieldDocumentation"/>.
        /// </summary>
        /// <param name="typeDocumentation">The documentation model of the type defining the field.</param>
        /// <param name="definition">The underlying Mono.Cecil definition of the field.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        /// <exception cref="ArgumentNullException">Thrown when one of the constructor arguments is null.</exception>
        internal FieldDocumentation(TypeDocumentation typeDocumentation, FieldDefinition definition, IXmlDocsProvider xmlDocsProvider)
            : base(typeDocumentation, definition?.ToMemberId(), xmlDocsProvider, definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            xmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));

            Type = definition.FieldType.ToTypeId();
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);

            Value = xmlDocsProvider.TryGetDocumentationComments(MemberId)?.Value ?? TextBlock.Empty;
        }
    }
}
