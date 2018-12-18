using System;
using System.Collections.Generic;
namespace MdDoc.Model.XmlDocs
{
    public sealed class MethodId : MemberId
    {
        public TypeId DefiningType { get; }

        public string Name { get; }

        public int Arity { get; }

        public IReadOnlyList<TypeId> Parameters { get; }

        public TypeId ReturnType { get; }


        public MethodId(TypeId definingType, string name) : this(definingType, name, 0, Array.Empty<TypeId>(), null)
        { }

        public MethodId(TypeId definingType, string name, IReadOnlyList<TypeId> parameters) : this(definingType, name, 0, parameters, null)
        { }

        public MethodId(TypeId definingType, string name, int arity, IReadOnlyList<TypeId> parameters) : this(definingType, name, arity, parameters, null)
        { }

        public MethodId(TypeId definingType, string name, int arity, IReadOnlyList<TypeId> parameters, TypeId returnType)
        {
            DefiningType = definingType ?? throw new ArgumentNullException(nameof(definingType));
            Name = name;
            Arity = arity;
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            ReturnType = returnType;
        }
    }
}
