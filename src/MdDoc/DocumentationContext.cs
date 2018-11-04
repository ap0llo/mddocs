using Grynwald.Utilities.Collections;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc
{
    class DocumentationContext
    {
        private readonly ModuleDefinition m_Module;
        private readonly HashSet<TypeReference> m_Types;
        private IDictionary<TypeReference, HashSet<MethodReference>> m_PropertyMethods = new Dictionary<TypeReference, HashSet<MethodReference>>();

        public IXmlDocProvider XmlDocProvider { get; }

        public DocumentationContext(ModuleDefinition module, IXmlDocProvider xmlDocProvider)
        {
            m_Module = module;
            XmlDocProvider = xmlDocProvider;
            m_Types = m_Module.Types.Where(t => t.IsPublic).Cast<TypeReference>().ToHashSet();                        
        }


        public bool IsDocumentedItem(TypeReference type) => m_Types.Contains(type);

        public bool IsDocumentedItem(MethodDefinition method) => 
            method.IsPublic && 
            IsDocumentedItem(method.DeclaringType) &&
            !IsPropertyMethod(method);


        public bool IsDocumentedItem(FieldDefinition field) =>
            field.IsPublic && 
            !field.Attributes.HasFlag(FieldAttributes.SpecialName) && 
            IsDocumentedItem(field.DeclaringType);

        public bool IsDocumentedItem(EventDefinition ev) =>
            (ev.AddMethod?.IsPublic == true || ev.RemoveMethod?.IsPublic == true) &&             
            IsDocumentedItem(ev.DeclaringType);

        public bool IsDocumentedItem(PropertyDefinition property) =>
            (property.GetMethod?.IsPublic == true || property.SetMethod?.IsPublic == true) &&
            IsDocumentedItem(property.DeclaringType);



        private bool IsPropertyMethod(MethodDefinition method)
        {            
            return m_PropertyMethods.GetOrAdd(
                method.DeclaringType, 
                () => GetPropertyMethods(method.DeclaringType).ToHashSet()
            )
            .Contains(method);            
        }

        private static IEnumerable<MethodReference> GetPropertyMethods(TypeDefinition type)
        {
            foreach (var property in type.Properties)
            {
                if (property.GetMethod != null)
                    yield return property.GetMethod;

                if (property.SetMethod != null)
                    yield return property.SetMethod;
            }
        }



    }
}
