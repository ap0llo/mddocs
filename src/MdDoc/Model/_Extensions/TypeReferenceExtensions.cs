using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
    public static class TypeReferenceExtensions
    {
        public static TypeName ToTypeName(this TypeReference typeReference) => new TypeName(typeReference);

    }
}
