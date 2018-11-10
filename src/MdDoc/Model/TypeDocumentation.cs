using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MdDoc.Model
{
    public class TypeDocumentation
    {
        private readonly DocumentationContext m_Context;

        public TypeKind Kind { get; }

        public TypeDefinition Definition { get; }

        public IReadOnlyCollection<PropertyDocumentation> Properties { get; }

        public IReadOnlyCollection<MethodDocumentation> Methods { get; }

        //TODO: Constructors, Fields, Operators, Events

        public TypeDocumentation(DocumentationContext context, TypeDefinition definition)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Kind = definition.Kind();

            Properties = definition.Properties
                .Where(m_Context.IsDocumentedItem)
                .Select(p => new PropertyDocumentation(m_Context, p))
                .ToArray();

            Methods = definition.GetDocumentedMethods(m_Context)
                .GroupBy(x => x.Name)
                .Select(x => new MethodDocumentation(m_Context, x))
                .ToArray();
        }

    }
}
