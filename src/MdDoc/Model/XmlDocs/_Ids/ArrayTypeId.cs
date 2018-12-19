using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model.XmlDocs
{
    public class ArrayTypeId : TypeId, IEquatable<ArrayTypeId>
    {
        public TypeId ElementType { get; }


        public ArrayTypeId(TypeId elementType) : base("System", "Array")
        {
            ElementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
        }

        
        
        public bool Equals(ArrayTypeId other)
        {
            return other != null && ElementType.Equals(other.ElementType);
        }

        public override bool Equals(object obj) => Equals(obj as ArrayTypeId);

        public override int GetHashCode()
        {
            unchecked
            {
                return ElementType.GetHashCode() * 397;
            }
        }      
    }
}
