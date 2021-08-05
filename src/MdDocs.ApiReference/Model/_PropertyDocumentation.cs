using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model of a property.
    /// </summary>
    public sealed class _PropertyDocumentation : _SimpleMemberDocumentation
    {
        /// <inheritdoc />
        public override string Name { get; }

        /// <inheritdoc />
        public override MemberId MemberId { get; }

        /// <inheritdoc />
        public override TypeId Type { get; }

        ///// <inheritdoc />
        // TODO 2021-08-05: public override string CSharpDefinition { get; }

        ///// <summary>
        ///// Gets the <c>value</c> documentation of the property.
        ///// </summary>
        // TODO 2021-08-05: public TextBlock? Value { get; }

        ///// <summary>
        ///// Gets the documented exceptions for the property.
        ///// </summary>
        // TODO 2021-08-05: public IReadOnlyList<ExceptionElement> Exceptions { get; }



        /// <summary>
        /// Initializes a new instance of <see cref="PropertyDocumentation"/>.
        /// </summary>
        /// <param name="typeDocumentation">The documentation model of the type defining the property.</param>
        /// <param name="definition">The underlying Mono.Cecil definition of the property.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        internal _PropertyDocumentation(_TypeDocumentation declaringType, string name, TypeId type) : base(declaringType)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value must not be null or whitespace", nameof(name));

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            Name = name;
            MemberId = new PropertyId(declaringType.TypeId, name);
            Type = type;

        }
    }
}
