using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.Utilities.Collections;
using MdDoc.Model;
using Mono.Cecil;

namespace MdDoc
{
    static class TypeDefinitionExtensions
    {
        private static object s_Lock = new object();
        private static IDictionary<TypeReference, HashSet<MethodReference>> s_PropertyMethods = new Dictionary<TypeReference, HashSet<MethodReference>>();


        public static IEnumerable<MethodDefinition> GetDocumentedConstrutors(this TypeDefinition type)
        {
            if (type.Kind() == TypeKind.Class || type.Kind() == TypeKind.Struct)
            {
                return type
                    .Methods
                    .Where(m => m.IsConstructor && m.IsPublic && !IsPropertyMethod(m));
            }
            else
            {
                return Enumerable.Empty<MethodDefinition>();
            }
        }

        public static IEnumerable<MethodDefinition> GetDocumentedMethods(this TypeDefinition type)
        {
            if (type.Kind() == TypeKind.Class || type.Kind() == TypeKind.Struct || type.Kind() == TypeKind.Interface)                
            {
                return type.Methods
                    .Where(m => !m.IsConstructor && m.IsPublic && !IsPropertyMethod(m));
            }
            else
            {
                return Enumerable.Empty<MethodDefinition>();
            }
        }


        private static bool IsPropertyMethod(MethodDefinition method)
        {
            lock (s_Lock)
            {
                return s_PropertyMethods.GetOrAdd(
                    method.DeclaringType,
                    () => GetPropertyMethods(method.DeclaringType).ToHashSet()
                )
                .Contains(method);
            }
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
