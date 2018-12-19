using System;
using System.Collections.Generic;
using System.Linq;

namespace MdDoc.Model.XmlDocs
{
    public abstract class TypeId : MemberId, IEquatable<TypeId>
    {
        public string NamespaceName { get; }

        public string Name { get; }
        

        public TypeId(string namespaceName, string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Value must not be null or empty", nameof(name));

            NamespaceName = namespaceName ?? throw new ArgumentNullException(nameof(namespaceName));
            Name = name;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = StringComparer.Ordinal.GetHashCode(NamespaceName) * 397;
                hash ^= StringComparer.Ordinal.GetHashCode(Name);             
                return hash;
            }
        }


        public bool Equals(TypeId other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return StringComparer.Ordinal.Equals(NamespaceName, other.NamespaceName) &&
                StringComparer.Ordinal.Equals(Name, other.Name);
        }
    }
}