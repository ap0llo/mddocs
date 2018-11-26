using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MdDoc.Model
{
    public class TypeDocumentation : IDocumentation
    {
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

        public IReadOnlyCollection<TypeReference> ImplementedInterfaces { get; }

        public IReadOnlyCollection<TypeReference> Attributes { get; }


        public TypeDocumentation(ModuleDocumentation moduleDocumentation, TypeDefinition definition)
        {
            ModuleDocumentation = moduleDocumentation ?? throw new ArgumentNullException(nameof(moduleDocumentation));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Kind = definition.Kind();

            Fields = definition.Fields
                .Where(field => field.IsPublic && !field.Attributes.HasFlag(FieldAttributes.SpecialName))                
                .Select(field => new FieldDocumentation(this, field))
                .ToArray();

            Events = definition.Events
                .Where(ev => (ev.AddMethod?.IsPublic == true || ev.RemoveMethod?.IsPublic == true))
                .Select(e => new EventDocumentation(this, e))
                .ToArray();

            Properties = definition.Properties
                .Where(property => (property.GetMethod?.IsPublic == true || property.SetMethod?.IsPublic == true))
                .Select(p => new PropertyDocumentation(this, p))
                .ToArray();

            var ctors = definition.GetDocumentedConstrutors().Select(x => new MethodOverload(x));
            if(ctors.Any())
                Constructors = new ConstructorDocumentation(this, ctors);

            Methods = definition.GetDocumentedMethods()
                .Where(m => !m.IsOperatorOverload())
                .GroupBy(x => x.Name)
                .Select(group => new MethodDocumentation(this, group.Select(x => new MethodOverload(x))))
                .ToArray();

            Operators = definition.GetDocumentedMethods()               
               .GroupBy(x => x.GetOperatorKind())
               .Where(group => group.Key.HasValue)
               .Select(group => new OperatorDocumentation(this, group.Select(x => new MethodOverload(x))))
               .ToArray();
            
            InheritanceHierarchy = LoadInheritanceHierarchy();
            Attributes = Definition.CustomAttributes.Select(x => x.AttributeType).ToArray();
            ImplementedInterfaces = LoadImplementedInterfaces();

        }


        public TypeDocumentation TryGetDocumentation(TypeReference typeReference) => ModuleDocumentation.TryGetDocumentation(typeReference);


        private IReadOnlyCollection<TypeReference> LoadInheritanceHierarchy()
        {
            if (Kind == TypeKind.Interface)
                return Array.Empty<TypeReference>();

            var inheritance = new LinkedList<TypeReference>();
            inheritance.AddFirst(Definition);
            
            var currentBaseType = Definition.BaseType.Resolve();
            while (currentBaseType != null)
            {
                inheritance.AddFirst(currentBaseType);
                currentBaseType = currentBaseType.BaseType?.Resolve();
            }

            return inheritance;
        }

        private IReadOnlyCollection<TypeReference> LoadImplementedInterfaces()
        {
            if (!Definition.HasInterfaces)
                return Array.Empty<TypeReference>();
            else
                return Definition.Interfaces.Select(x => x.InterfaceType).ToArray();
        }
    }
}
