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

        public IReadOnlyCollection<FieldDocumentation> Fields { get; }

        public IReadOnlyCollection<EventDocumentation> Events { get; }

        public IReadOnlyCollection<PropertyDocumentation> Properties { get; }

        public MethodDocumentation Constructors { get; }

        public IReadOnlyCollection<MethodDocumentation> Methods { get; }

        public IReadOnlyCollection<OperatorDocumentation> Operators { get; }
        

        public TypeDocumentation(DocumentationContext context, TypeDefinition definition)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Kind = definition.Kind();

            Fields = definition.Fields
                .Where(m_Context.IsDocumentedItem)
                .Select(field => new FieldDocumentation(m_Context, field))
                .ToArray();

            Events = definition.Events
                .Where(m_Context.IsDocumentedItem)
                .Select(e => new EventDocumentation(m_Context, e))
                .ToArray();

            Properties = definition.Properties
                .Where(m_Context.IsDocumentedItem)
                .Select(p => new PropertyDocumentation(m_Context, p))
                .ToArray();

            var ctors = definition.GetDocumentedConstrutors(m_Context);
            if(ctors.Any())
                Constructors = new MethodDocumentation(m_Context, ctors);

            Methods = definition.GetDocumentedMethods(m_Context)
                .Where(m => !m.IsOperatorOverload())
                .GroupBy(x => x.Name)
                .Select(x => new MethodDocumentation(m_Context, x))
                .ToArray();

            Operators = definition.GetDocumentedMethods(m_Context)               
               .GroupBy(x => x.GetOperatorKind())
               .Where(group => group.Key.HasValue)
               .Select(group => new OperatorDocumentation(m_Context, group))
               .ToArray();
        }

    }
}
