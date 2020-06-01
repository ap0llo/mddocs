using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Identifies a array type.
    /// </summary>
    public sealed class ArrayTypeId : TypeId, IEquatable<ArrayTypeId>
    {
        /// <summary>
        /// Gets the array's element type.
        /// </summary>
        public TypeId ElementType { get; }

        /// <summary>
        /// Gets the array's number of dimensions.
        /// </summary>
        public int Dimensions { get; }

        /// <inheritdoc />
        public override string DisplayName => $"{ElementType.DisplayName}[{new String(',', Dimensions - 1)}]";

        /// <inheritdoc />
        public override bool IsVoid => false;


        /// <summary>
        /// Initializes a new one-dimensional array.
        /// </summary>
        /// <param name="elementType">The array's element type.</param>
        public ArrayTypeId(TypeId elementType) : this(elementType, 1)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="ArrayTypeId"/>.
        /// </summary>
        /// <param name="elementType">The array's element type.</param>
        /// <param name="dimensions">The array's number of dimensions.</param>
        public ArrayTypeId(TypeId elementType, int dimensions) : base(new NamespaceId("System"), "Array")
        {
            ElementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
            Dimensions = dimensions >= 1
                ? dimensions
                : throw new ArgumentOutOfRangeException(nameof(dimensions), "Value needs to be equal or greater than 1");
        }


        /// <inheritdoc />
        public override bool Equals(TypeId? other) => Equals(other as ArrayTypeId);

        /// <inheritdoc />
        public override bool Equals(object? obj) => Equals(obj as ArrayTypeId);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = ElementType.GetHashCode() * 397;
                hash ^= Dimensions.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Determines whether this instance of <see cref="ArrayTypeId"/> refers to the same type as <paramref name="other"/>.
        /// </summary>
        public bool Equals(ArrayTypeId? other)
        {
            return other != null &&
                   ElementType.Equals(other.ElementType) &&
                   Dimensions == other.Dimensions;
        }
    }
}
