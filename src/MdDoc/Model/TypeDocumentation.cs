using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
    public class TypeDocumentation
    {

        public TypeReference TypeReference { get; }

        public TypeDocumentation(DocumentationContext context, TypeReference typeReference)
        {
            TypeReference = typeReference ?? throw new ArgumentNullException(nameof(typeReference));
        }

    }
}
