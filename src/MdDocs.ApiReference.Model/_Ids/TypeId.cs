using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public abstract class TypeId : MemberId, IEquatable<TypeId>
    {
        public NamespaceId Namespace { get; }

        public string Name { get; }        

        public abstract string DisplayName { get; }

        public abstract bool IsVoid { get; }

        protected string NamespaceAndName => String.IsNullOrEmpty(Namespace.Name) ? Name : $"{Namespace.Name}.{Name}";


        public TypeId(NamespaceId @namespace, string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Value must not be null or empty", nameof(name));

            Namespace = @namespace ?? throw new ArgumentNullException(nameof(@namespace));
            Name = name;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = Namespace.GetHashCode() * 397;
                hash ^= StringComparer.Ordinal.GetHashCode(Name);             
                return hash;
            }
        }


        public virtual bool Equals(TypeId other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return Namespace.Equals(other.Namespace) &&
                StringComparer.Ordinal.Equals(Name, other.Name);
        }
    }
}
