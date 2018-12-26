using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.Utilities.Collections;
using Mono.Cecil;

namespace MdDoc.Model
{
    static class TypeDefinitionExtensions
    {
        private static readonly object s_Lock = new object();
        private static IDictionary<TypeReference, HashSet<MethodReference>> s_PropertyAccessors = new Dictionary<TypeReference, HashSet<MethodReference>>();
        private static IDictionary<TypeReference, HashSet<MethodReference>> s_EventAccessors = new Dictionary<TypeReference, HashSet<MethodReference>>();


        public static TypeKind Kind(this TypeDefinition type)
        {
            if (type.IsEnum)
                return TypeKind.Enum;

            if (type.IsClass)
                return type.IsValueType ? TypeKind.Struct : TypeKind.Class;

            if (type.IsInterface)
                return TypeKind.Interface;

            
            throw new InvalidOperationException();
        }

        public static IEnumerable<MethodDefinition> GetDocumentedConstrutors(this TypeDefinition type)
        {
            if (type.Kind() == TypeKind.Class || type.Kind() == TypeKind.Struct)
            {
                return type
                    .Methods
                    .Where(m => m.IsConstructor && m.IsPublic && !IsPropertyAccessor(m));
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
                    .Where(m => !m.IsConstructor && m.IsPublic && !IsPropertyAccessor(m) && !IsEventAccessor(m));
            }
            else
            {
                return Enumerable.Empty<MethodDefinition>();
            }
        }

        /// <summary>
        /// Gets a type's custom attributes excluding attributes emitted by the C# compiler not relevant for the user
        /// </summary>
        /// <returns>
        /// Returns all attributes except
        /// <list type="bullet">
        ///     <item><c>DefaultMemberAttribute</c> for classes</item>
        ///     <item><c>ExtensionAttribute</c> (indicating that the class defines extension methods) for classes</item>
        ///     <item><c>IsReadOnlyAttribute</c> for structs indicating that it is a <c>readonly struct</c></item>
        /// </list>
        /// </returns>
        public static IEnumerable<CustomAttribute> GetCustomAttributes(this TypeDefinition type)
        {
            var typeKind = type.Kind();
            
            return type.CustomAttributes
                .Where(attribute =>
                {
                    if (typeKind == TypeKind.Class && attribute.AttributeType.FullName == Constants.DefaultMemberAttributeFullName)
                        return false;

                    if (typeKind == TypeKind.Class && attribute.AttributeType.FullName == Constants.ExtensionAttributeFullName)
                        return false;

                    if (typeKind == TypeKind.Struct && attribute.AttributeType.FullName == Constants.IsReadOnlyAttributeFullName)
                        return false;

                    return true;
                });
        }



        private static bool IsPropertyAccessor(MethodDefinition method)
        {
            lock (s_Lock)
            {
                return s_PropertyAccessors.GetOrAdd(
                    method.DeclaringType,
                    () => GetPropertyAccessors(method.DeclaringType).ToHashSet()
                )
                .Contains(method);
            }
        }

        private static bool IsEventAccessor(MethodDefinition method)
        {
            lock (s_Lock)
            {
                return s_EventAccessors.GetOrAdd(
                    method.DeclaringType,
                    () => GetEventAccessors(method.DeclaringType).ToHashSet()
                )
                .Contains(method);
            }
        }

        private static IEnumerable<MethodReference> GetPropertyAccessors(TypeDefinition type)
        {
            foreach (var property in type.Properties)
            {
                if (property.GetMethod != null)
                    yield return property.GetMethod;

                if (property.SetMethod != null)
                    yield return property.SetMethod;
            }
        }

        private static IEnumerable<MethodReference> GetEventAccessors(TypeDefinition type)
        {
            foreach (var ev in type.Events)
            {
                if (ev.AddMethod != null)
                    yield return ev.AddMethod;

                if (ev.RemoveMethod != null)
                    yield return ev.RemoveMethod;
            }
        }
    }
}
