using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model for an event.
    /// </summary>
    public sealed class _EventDocumentation : _SimpleMemberDocumentation
    {
        /// <inheritdoc />
        public override string Name { get; }

        /// <inheritdoc />
        public override MemberId MemberId { get; }

        ///// <inheritdoc />
        // TODO 2021-08-05: public override string CSharpDefinition { get; }

        /// <inheritdoc />
        public override TypeId Type { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="EventDocumentation"/>.
        /// </summary>
        /// <param name="typeDocumentation">The documentation model of the type defining the event.</param>
        /// <param name="definition">The underlying Mono.Cecil definition of the event.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        /// <exception cref="ArgumentNullException">Thrown when one of the constructor arguments is null.</exception>
        internal _EventDocumentation(_TypeDocumentation declaringType, string name, TypeId type) : base(declaringType)
        {
            if (declaringType is null)
                throw new ArgumentNullException(nameof(declaringType));

            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value must not be null or whitespace", nameof(name));

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            Name = name;
            MemberId = new EventId(declaringType.TypeId, name);
            Type = type;
        }
    }
}
