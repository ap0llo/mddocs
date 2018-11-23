using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MdDoc.Model
{
    public class TypeDocumentation : IDocumentation
    {
        private readonly DocumentationContext m_Context;

        public ModuleDocumentation ModuleDocumentation { get; }

        public string Name => Definition.Name;

        public string Namespace => Definition.Namespace;

        public string AssemblyName => Definition.Module.Assembly.Name.Name;

        public TypeKind Kind { get; }
        
        public TypeDefinition Definition { get; }    

        public IReadOnlyCollection<FieldDocumentation> Fields { get; }

        public IReadOnlyCollection<EventDocumentation> Events { get; }

        public IReadOnlyCollection<PropertyDocumentation> Properties { get; }

        public ConstructorDocumentation Constructors { get; }

        public IReadOnlyCollection<MethodDocumentation> Methods { get; }

        public IReadOnlyCollection<OperatorDocumentation> Operators { get; }
        
        public IReadOnlyCollection<TypeReference> InheritanceHierarchy { get; }

        public IReadOnlyCollection<TypeReference> Attributes { get; }


        public TypeDocumentation(ModuleDocumentation moduleDocumentation, DocumentationContext context, TypeDefinition definition)
        {
            ModuleDocumentation = moduleDocumentation ?? throw new ArgumentNullException(nameof(moduleDocumentation));
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Kind = definition.Kind();

            Fields = definition.Fields
                .Where(m_Context.IsDocumentedItem)
                .Select(field => new FieldDocumentation(this, m_Context, field))
                .ToArray();

            Events = definition.Events
                .Where(m_Context.IsDocumentedItem)
                .Select(e => new EventDocumentation(this, m_Context, e))
                .ToArray();

            Properties = definition.Properties
                .Where(m_Context.IsDocumentedItem)
                .Select(p => new PropertyDocumentation(this, m_Context, p))
                .ToArray();

            var ctors = definition.GetDocumentedConstrutors(m_Context);
            if(ctors.Any())
                Constructors = new ConstructorDocumentation(this, m_Context, ctors);

            Methods = definition.GetDocumentedMethods(m_Context)
                .Where(m => !m.IsOperatorOverload())
                .GroupBy(x => x.Name)
                .Select(x => new MethodDocumentation(this, m_Context, x))
                .ToArray();

            Operators = definition.GetDocumentedMethods(m_Context)               
               .GroupBy(x => x.GetOperatorKind())
               .Where(group => group.Key.HasValue)
               .Select(group => new OperatorDocumentation(this, m_Context, group))
               .ToArray();
            
            InheritanceHierarchy = LoadInheritanceHierarchy();
            Attributes = Definition.CustomAttributes.Select(x => x.AttributeType).ToArray();
        }


        public TypeDocumentation TryGetDocumentation(TypeReference typeReference) => ModuleDocumentation.TryGetDocumentation(typeReference);


        private IReadOnlyCollection<TypeReference> LoadInheritanceHierarchy()
        {
            var inheritance = new LinkedList<TypeReference>();
            inheritance.AddFirst(Definition);

            if (Kind == TypeKind.Interface)
            {
                return inheritance;
            }
            
            var currentBaseType = Definition.BaseType.Resolve();
            while (currentBaseType != null)
            {
                inheritance.AddFirst(currentBaseType);
                currentBaseType = currentBaseType.BaseType?.Resolve();
            }

            return inheritance;
        }

    }
}
