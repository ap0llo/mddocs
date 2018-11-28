using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MdDoc.Model
{
    public class TypeDocumentation : IDocumentation
    {
        public ModuleDocumentation ModuleDocumentation { get; }

        public TypeName Name { get; }
        
        public string AssemblyName => Definition.Module.Assembly.Name.Name;
        
        public TypeKind Kind { get; }
        
        public TypeDefinition Definition { get; }    

        public IReadOnlyCollection<FieldDocumentation> Fields { get; }

        public IReadOnlyCollection<EventDocumentation> Events { get; }

        public IReadOnlyCollection<PropertyDocumentation> Properties { get; }

        public ConstructorDocumentation Constructors { get; }

        public IReadOnlyCollection<MethodDocumentation> Methods { get; }

        public IReadOnlyCollection<OperatorDocumentation> Operators { get; }
        
        public IReadOnlyCollection<TypeName> InheritanceHierarchy { get; }

        public IReadOnlyCollection<TypeName> ImplementedInterfaces { get; }

        public IReadOnlyCollection<TypeName> Attributes { get; }


        public TypeDocumentation(ModuleDocumentation moduleDocumentation, TypeDefinition definition)
        {
            ModuleDocumentation = moduleDocumentation ?? throw new ArgumentNullException(nameof(moduleDocumentation));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Kind = definition.Kind();
            Name = new TypeName(definition);

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

            var ctors = definition.GetDocumentedConstrutors();
            if(ctors.Any())
                Constructors = new ConstructorDocumentation(this, ctors);

            Methods = definition.GetDocumentedMethods()
                .Where(m => !m.IsOperatorOverload())
                .GroupBy(x => x.Name)
                .Select(group => new MethodDocumentation(this, group))
                .ToArray();

            Operators = definition.GetDocumentedMethods()               
               .GroupBy(x => x.GetOperatorKind())
               .Where(group => group.Key.HasValue)
               .Select(group => new OperatorDocumentation(this, group))
               .ToArray();
            
            InheritanceHierarchy = LoadInheritanceHierarchy();
            Attributes = Definition.CustomAttributes.Select(x => new TypeName(x.AttributeType)).ToArray();
            ImplementedInterfaces = LoadImplementedInterfaces();

        }


        public TypeDocumentation TryGetDocumentation(TypeName type) => ModuleDocumentation.TryGetDocumentation(type);


        private IReadOnlyCollection<TypeName> LoadInheritanceHierarchy()
        {
            if (Kind == TypeKind.Interface)
                return Array.Empty<TypeName>();

            var inheritance = new LinkedList<TypeName>();
            inheritance.AddFirst(Name);
            
            var currentBaseType = Definition.BaseType.Resolve();
            while (currentBaseType != null)
            {
                inheritance.AddFirst(new TypeName(currentBaseType));
                currentBaseType = currentBaseType.BaseType?.Resolve();
            }

            return inheritance;
        }

        private IReadOnlyCollection<TypeName> LoadImplementedInterfaces()
        {
            if (!Definition.HasInterfaces)
                return Array.Empty<TypeName>();
            else
                return Definition.Interfaces.Select(x => new TypeName(x.InterfaceType)).ToArray();
        }
    }
}
