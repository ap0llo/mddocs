using System;

namespace MdDoc.Model
{
    public sealed class ArrayTypeId : TypeId, IEquatable<ArrayTypeId>
    {
        public TypeId ElementType { get; }

        public int Dimensions { get; }

        public override string DisplayName => $"{ElementType.DisplayName}[{new String(',', Dimensions - 1)}]";

        public override bool IsVoid => false;


        public ArrayTypeId(TypeId elementType) : this(elementType, 1)
        { }

        public ArrayTypeId(TypeId elementType, int dimensions) : base("System", "Array")
        {
            ElementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
            Dimensions = dimensions >= 1 
                ? dimensions 
                : throw new ArgumentOutOfRangeException(nameof(dimensions), "Value needs to be equal or greater than 1");
        }


        public override bool Equals(TypeId other) => Equals(other as ArrayTypeId);

        public override bool Equals(object obj) => Equals(obj as ArrayTypeId);

        public bool Equals(ArrayTypeId other)
        {
            return other != null && ElementType.Equals(other.ElementType) && Dimensions == other.Dimensions;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = ElementType.GetHashCode() * 397;
                hash ^= Dimensions.GetHashCode();
                return hash;
            }
        }      
    }
}
