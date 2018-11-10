using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model
{
    public class TypeDocumentation
    {

        public TypeKind Kind { get; }

        public TypeDefinition Definition { get; }


        public TypeDocumentation(DocumentationContext context, TypeDefinition typeReference)
        {
            Definition = typeReference ?? throw new ArgumentNullException(nameof(typeReference));
            Kind = typeReference.Kind();
        }

    }
}
