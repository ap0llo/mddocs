using System;

namespace MdDoc.Model
{
    /// <summary>
    /// Identifies an unbound generic type
    /// </summary>
    public sealed class GenericTypeId : TypeId, IEquatable<GenericTypeId>
    {
        public int Arity { get; }


        public GenericTypeId(string namespaceName, string name, int arity) : base(namespaceName, name)
        {
            Arity = arity;
        }


        public override bool Equals(object obj) => Equals(obj as GenericTypeId);

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = base.GetHashCode();
                hash ^= Arity.GetHashCode();                
                return hash;
            }
        }

        public bool Equals(GenericTypeId other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return Equals((TypeId) other) && Arity == other.Arity;
        }
    }
}
