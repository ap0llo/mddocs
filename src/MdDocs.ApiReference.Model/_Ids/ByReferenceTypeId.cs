using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Identifies a "by reference type", typically used for method <c>ref</c> and <c>out</c> parameters.
    /// </summary>
    public sealed class ByReferenceTypeId : TypeId, IEquatable<ByReferenceTypeId>
    {
        /// <inheritdoc />
        public override string DisplayName => ElementType.DisplayName;

        /// <inheritdoc />
        public override bool IsVoid => false;

        /// <summary>
        /// Gets the type of the value being passed by reference.
        /// </summary>
        public TypeId ElementType { get; }


        public ByReferenceTypeId(TypeId elementType) : base(elementType.Namespace, $"{elementType.Name}&")
        {
            ElementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
        }


        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as ByReferenceTypeId);

        /// <inheritdoc />
        public override bool Equals(TypeId other) => other is ByReferenceTypeId && base.Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Determines whether this instance of <see cref="ByReferenceTypeId"/> refers to the same type as <paramref name="other"/>.
        /// </summary>
        public bool Equals(ByReferenceTypeId other) => other != null && ElementType.Equals(other.ElementType);


    }
}
