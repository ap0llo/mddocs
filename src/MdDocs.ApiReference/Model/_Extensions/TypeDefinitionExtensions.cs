using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.Utilities.Collections;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Extension methods for <see cref="TypeDefinition"/>.
    /// </summary>
    internal static class TypeDefinitionExtensions
    {
        private static readonly object s_Lock = new object();
        private static readonly IDictionary<TypeReference, HashSet<MethodReference>> s_PropertyAccessors = new Dictionary<TypeReference, HashSet<MethodReference>>();
        private static readonly IDictionary<TypeReference, HashSet<MethodReference>> s_EventAccessors = new Dictionary<TypeReference, HashSet<MethodReference>>();


        /// <summary>
        /// Gets the kind of type (class, interface, struct ...) for the specified definition.
        /// </summary>
        /// <seealso cref="TypeKind"/>
        public static TypeKind Kind(this TypeDefinition type)
        {
            if (type.IsEnum)
                return TypeKind.Enum;

            if (type.IsClass)
                return type.IsValueType ? TypeKind.Struct : TypeKind.Class;

            if (type.IsInterface)
                return TypeKind.Interface;

            //TODO: Delegates
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the public constructors for the specified type.
        /// </summary>
        /// <param name="type">The type to get the constructors for.</param>
        /// <returns>Returns the definitions of the type's constructors or an empty enumerable if no constructors were found.</returns>
        public static IEnumerable<MethodDefinition> GetPublicConstrutors(this TypeDefinition type)
        {
            // only classes and structs have constructors we case about
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

        /// <summary>
        /// Gets the public methods of the specified type.
        /// </summary>
        /// <param name="type">The type which's methods to get.</param>
        /// <remarks>
        /// <c>GetPublicMethods</c> returns all public methods for the type but excludes methods that are
        /// constructors, property getters/setters or event accessors.
        /// </remarks>
        /// <returns>Returns the definitions of the type's methods or an empty enumerable if no methods were found.</returns>
        /// <seealso cref="IsPropertyAccessor(MethodDefinition)"/>
        /// <seealso cref="IsEventAccessor(MethodDefinition)"/>
        public static IEnumerable<MethodDefinition> GetPublicMethods(this TypeDefinition type)
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
        /// Gets a type's public custom attributes excluding attributes emitted by the C# compiler not relevant for the user.
        /// </summary>
        /// <returns>
        /// Returns all attributes except:
        /// <list type="bullet">
        ///     <item><c>DefaultMemberAttribute</c> for classes.</item>
        ///     <item><c>ExtensionAttribute</c> (indicating that the class defines extension methods) for classes.</item>
        ///     <item><c>IsReadOnlyAttribute</c> for structs indicating that it is a <c>readonly struct</c>.</item>
        ///     <item>non-public Attribute types</item>
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

                    if (!attribute.AttributeType.Resolve().IsPublic)
                        return false;


                    return true;
                });
        }


        /// <summary>
        /// Determines whether the specified methods is a property getter or setter.
        /// </summary>
        private static bool IsPropertyAccessor(MethodDefinition method)
        {
            lock (s_Lock)
            {
                // on the first call of IsPropertyAccessor() for a type
                // load all the type's getters and setters and cache them for
                // subsequent calls.
                return s_PropertyAccessors.GetOrAdd(
                    method.DeclaringType,
                    () => GetPropertyAccessors(method.DeclaringType).ToHashSet()
                )
                .Contains(method);
            }
        }

        /// <summary>
        /// Determines whether the specified method is an event's <c>add</c> or <c>remove</c> method.
        /// </summary>
        private static bool IsEventAccessor(MethodDefinition method)
        {
            lock (s_Lock)
            {
                // on the first call of IsEventAccessor() for a type
                // load all the type's event accessors and cache them for
                // subsequent calls.
                return s_EventAccessors.GetOrAdd(
                    method.DeclaringType,
                    () => GetEventAccessors(method.DeclaringType).ToHashSet()
                )
                .Contains(method);
            }
        }

        /// <summary>
        /// Gets all the type's property accessors (<c>get</c> and <c>set</c> methods).
        /// </summary>
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

        /// <summary>
        /// Gets all the type's event accessors (<c>add</c> and <c>remove</c> methods).
        /// </summary>
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
