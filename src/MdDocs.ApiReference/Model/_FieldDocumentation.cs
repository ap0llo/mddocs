using System;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model for a field.
    /// </summary>
    public sealed class _FieldDocumentation : _SimpleMemberDocumentation
    {
        /// <inheritdoc />
        public override string Name { get; }

        /// <inheritdoc />
        public override MemberId MemberId { get; }

        ///// <inheritdoc />
        // TODO 2021-08-05: public override string CSharpDefinition { get; }

        /// <inheritdoc />
        public override TypeId Type { get; }

        ///// <summary>
        ///// Gets the <c>value</c> documentation for the field.
        ///// </summary>
        // TODO 2021-08-05: public TextBlock Value { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="FieldDocumentation"/>.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        internal _FieldDocumentation(_TypeDocumentation declaringType, string name, TypeId type) : base(declaringType)
        {
            if (declaringType is null)
                throw new ArgumentNullException(nameof(declaringType));

            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value must not be null or whitespace", nameof(name));

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            MemberId = new FieldId(declaringType.TypeId, name);
            Name = name;
            Type = type;
        }
    }
}
