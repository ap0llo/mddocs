using System;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model for an event.
    /// </summary>
    public sealed class EventDocumentation : SimpleMemberDocumentation
    {
        /// <inheritdoc />
        public override string Name => Definition.Name;

        /// <inheritdoc />
        public override string CSharpDefinition { get; }

        /// <inheritdoc />
        public override TypeId Type { get; }

        /// <summary>
        /// Gets the underlying Mono.Cecil definition of the event.
        /// </summary>
        internal EventDefinition Definition { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="EventDocumentation"/>.
        /// </summary>
        /// <param name="typeDocumentation">The documentation model of the type defining the event.</param>
        /// <param name="definition">The underlying Mono.Cecil definition of the event.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        /// <exception cref="ArgumentNullException">Thrown when one of the constructor arguments is null.</exception>
        internal EventDocumentation(TypeDocumentation typeDocumentation, EventDefinition definition, IXmlDocsProvider xmlDocsProvider)
            : base(typeDocumentation, definition.ToMemberId(), xmlDocsProvider, definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));

            Type = definition.EventType.ToTypeId();
            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);
        }
    }
}
