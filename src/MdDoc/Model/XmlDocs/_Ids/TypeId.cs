using System;
using System.Collections.Generic;

namespace MdDoc.Model.XmlDocs
{
    public sealed class TypeId : MemberId
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
    }
}
