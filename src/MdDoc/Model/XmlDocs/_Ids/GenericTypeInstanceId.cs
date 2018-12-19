using System;
using System.Collections.Generic;
using System.Linq;

namespace MdDoc.Model.XmlDocs
{
    /// <summary>
    /// Identifies a constructed type (a generic type with type arguments)
    /// </summary>
    public class GenericTypeInstanceId : TypeId, IEquatable<GenericTypeInstanceId>
    {
        public IReadOnlyList<TypeId> TypeArguments { get; }


        public GenericTypeInstanceId(string namespaceName, string name, IReadOnlyList<TypeId> typeArguments) : base(namespaceName, name)
        {
            TypeArguments = typeArguments ?? throw new ArgumentNullException(nameof(typeArguments));
        }


        public override bool Equals(object obj) => Equals(obj as GenericTypeInstanceId);

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = base.GetHashCode();

                foreach (var argument in TypeArguments)
                {
                    hash ^= argument.GetHashCode();
                }

                return hash;
            }
        }

        public bool Equals(GenericTypeInstanceId other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return Equals((TypeId) other) && TypeArguments.SequenceEqual(other.TypeArguments);
        }
    }
}