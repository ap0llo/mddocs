using System;
using System.Collections.Generic;
using System.Linq;

namespace MdDoc.Model.XmlDocs
{
    public sealed class TypeId : MemberId, IEquatable<TypeId>
    {
        public string NamespaceName { get; }

        public string Name { get; }

        public int Arity { get; }

        public IReadOnlyList<TypeId> TypeArguments { get; }


        public TypeId(string namespaceName, string name) : this(namespaceName, name, 0, Array.Empty<TypeId>())
        { }

        public TypeId(string namespaceName, string name, int arity) : this(namespaceName, name, arity, Array.Empty<TypeId>())
        { }

        public TypeId(string namespaceName, string name, IReadOnlyList<TypeId> typeArguments) : this(namespaceName, name, typeArguments?.Count ?? 0, typeArguments)
        { }

        private TypeId(string namespaceName, string name, int arity, IReadOnlyList<TypeId> typeArguments)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Value must not be null or empty", nameof(name));

            if(namespaceName == null)
                throw new ArgumentNullException(nameof(namespaceName));

            if (typeArguments == null)
                throw new ArgumentNullException(nameof(typeArguments));

            if (typeArguments.Count > 0 && typeArguments.Count != arity)
                throw new ArgumentException("When specifying type arguments, the number of arguments has to match the arity");

            NamespaceName = namespaceName;
            Name = name;
            Arity = arity;
            TypeArguments = typeArguments;
        }


        public override bool Equals(object obj) => Equals(obj as TypeId);

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = StringComparer.Ordinal.GetHashCode(NamespaceName) * 397;
                hash ^= StringComparer.Ordinal.GetHashCode(Name);
                
                foreach(var arguemnt in TypeArguments)
                {
                    hash ^= arguemnt.GetHashCode();
                }

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
                StringComparer.Ordinal.Equals(Name, other.Name) &&
                Arity == other.Arity &&
                TypeArguments.SequenceEqual(other.TypeArguments);
        }
    }
}
