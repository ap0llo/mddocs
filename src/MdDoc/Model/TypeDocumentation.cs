using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Model
{
    public class TypeDocumentation
    {
        public TypeKind Kind { get; }

        public TypeDefinition Definition { get; }

        public IReadOnlyCollection<PropertyDocumentation> Properties { get; }

        //TODO: Constructors, Fields, Methods, Operators, Events

        public TypeDocumentation(DocumentationContext context, TypeDefinition definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Kind = definition.Kind();

            Properties = definition.Properties
                .Where(context.IsDocumentedItem)
                .Select(p => new PropertyDocumentation(context, p))
                .ToArray();
        }

    }
}
