using System;
using System.Collections.Generic;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model of a type.
    /// </summary>
    public sealed class _TypeDocumentation
    //TODO 2021-08-04: implement IAssemblyMemberDocumentation and IObsoleteableDocumentation ?
    {
        //TODO 2021-08-04: private readonly IXmlDocsProvider m_XmlDocsProvider;
        //TODO 2021-08-04: private readonly ILogger m_Logger;
        //TODO 2021-08-04: private readonly IDictionary<MemberId, FieldDocumentation> m_Fields;
        //TODO 2021-08-04: private readonly IDictionary<MemberId, EventDocumentation> m_Events;
        //TODO 2021-08-04: private readonly IDictionary<MemberId, PropertyDocumentation> m_Properties;
        //TODO 2021-08-04: private readonly IDictionary<string, IndexerDocumentation> m_Indexers;
        //TODO 2021-08-04: private readonly IDictionary<string, MethodDocumentation> m_Methods;
        //TODO 2021-08-04: private readonly IDictionary<OperatorKind, OperatorDocumentation> m_Operators;
        private readonly List<_TypeDocumentation> m_NestedTypes = new List<_TypeDocumentation>();


        /// <summary>
        /// Gets the id of the type.
        /// </summary>
        public MemberId MemberId => TypeId;

        /// <summary>
        /// Gets the id of the type.
        /// </summary>
        public TypeId TypeId { get; }

        /// <summary>
        /// Gets the documentation model of the Assembly that defines this type.
        /// </summary>
        public _AssemblyDocumentation Assembly { get; }

        /// <summary>
        /// Gets the documentation model of this type's namespace.
        /// </summary>
        public _NamespaceDocumentation Namespace { get; }

        /// <summary>
        /// Gets the type's display name
        /// </summary>
        public string DisplayName => TypeId.DisplayName;

        /// <summary>
        /// Gets the name of the assembly this type is defined in.
        /// </summary>
        public string AssemblyName => Assembly.Name;

        ///// <summary>
        ///// Gets the kind of the type (class, struct, interface ...)
        ///// </summary>
        //TODO 2021-08-04: public TypeKind Kind { get; }

        ///// <summary>
        ///// Gets the type's fields.
        ///// </summary>
        //TODO 2021-08-04: public IReadOnlyCollection<FieldDocumentation> Fields { get; }

        ///// <summary>
        ///// Gets the type's events.
        ///// </summary>
        //TODO 2021-08-04: public IReadOnlyCollection<EventDocumentation> Events { get; }

        ///// <summary>
        ///// Gets the type's properties.
        ///// </summary>
        //TODO 2021-08-04: public IReadOnlyCollection<PropertyDocumentation> Properties { get; }

        ///// <summary>
        ///// Gets the type's indexers.
        ///// </summary>
        //TODO 2021-08-04: public IReadOnlyCollection<IndexerDocumentation> Indexers { get; }

        ///// <summary>
        ///// Gets the type's constructors.
        ///// </summary>
        //TODO 2021-08-04: public ConstructorDocumentation? Constructors { get; }

        ///// <summary>
        ///// Gets the type's methods.
        ///// </summary>
        //TODO 2021-08-04: public IReadOnlyCollection<MethodDocumentation> Methods { get; }

        ///// <summary>
        ///// Gets the type's operator overloads.
        ///// </summary>
        //TODO 2021-08-04: public IReadOnlyCollection<OperatorDocumentation> Operators { get; }

        ///// <summary>
        ///// Gets the type's base types.
        ///// </summary>
        //TODO 2021-08-04: public IReadOnlyCollection<TypeId> InheritanceHierarchy { get; }

        ///// <summary>
        ///// Gets the interfaces this type implements.
        ///// </summary>
        //TODO 2021-08-04: public IReadOnlyCollection<TypeId> ImplementedInterfaces { get; }

        ///// <summary>
        ///// Gets the type's custom attributes.
        ///// </summary>
        //TODO 2021-08-04: public IReadOnlyCollection<TypeId> Attributes { get; }

        ///// <summary>
        ///// Gets the type's <c>summary</c> documentation.
        ///// </summary>
        //TODO 2021-08-04: public TextBlock Summary { get; }

        ///// <summary>
        ///// Gets the type's <c>remarks</c> documentation.
        ///// </summary>
        //TODO 2021-08-04: public TextBlock Remarks { get; }

        ///// <summary>
        ///// Gets the type's <c>seealso</c> documentation items.
        ///// </summary>
        //TODO 2021-08-04: public IReadOnlyCollection<SeeAlsoElement> SeeAlso { get; }

        ///// <summary>
        ///// Gets the type's generic type parameters.
        ///// </summary>
        //TODO 2021-08-04: public IReadOnlyCollection<TypeParameterDocumentation> TypeParameters { get; }

        ///// <summary>
        ///// Gets the definition of the type as C# code.
        ///// </summary>
        //TODO 2021-08-04: public string CSharpDefinition { get; } Rename to Syntax??

        ///// <summary>
        ///// Gets the type's <c>example</c> documentation items.
        ///// </summary>
        //TODO 2021-08-04: public TextBlock Example { get; }

        ///// <inheritdoc />
        //TODO 2021-08-04:  public bool IsObsolete { get; }

        ///// <inheritdoc />
        //TODO 2021-08-04: public string? ObsoleteMessage { get; }

        /// <summary>
        /// Gets whether this type is a nested type
        /// </summary>
        public bool IsNestedType => DeclaringType != null;

        /// <summary>
        /// Gets the model object for the type this type is defined in if it is a nested type.
        /// </summary>
        /// <value>
        /// The model class for the declaring type if the type is a nested type, otherwise <c>null</c>.
        /// </value>
        public _TypeDocumentation? DeclaringType { get; }

        /// <summary>
        /// Gets the type's nested types
        /// </summary>
        public IReadOnlyCollection<_TypeDocumentation> NestedTypes => m_NestedTypes;


        /// <summary>
        /// Initializes a new instance of <see cref="_TypeDocumentation"/> for a top-level type.
        /// </summary>
        internal _TypeDocumentation(_AssemblyDocumentation assembly, _NamespaceDocumentation @namespace, TypeId typeId)
        {
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));

            if (@namespace is null)
                throw new ArgumentNullException(nameof(@namespace));

            if (typeId is null)
                throw new ArgumentNullException(nameof(typeId));

            if (!typeId.Namespace.Equals(@namespace.NamespaceId))
                throw new InconsistentModelException($"Mismatch between namespace of type '{typeId}' and id of parent namespace '{@namespace.NamespaceId}'");

            Assembly = assembly;
            Namespace = @namespace;
            TypeId = typeId;
            DeclaringType = null;
        }


        /// <summary>
        /// Initializes a new instance of <see cref="_TypeDocumentation"/> for a nested type.
        /// </summary>
        internal _TypeDocumentation(_AssemblyDocumentation assembly, _TypeDocumentation declaringType, TypeId typeId)
        {
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));

            if (declaringType is null)
                throw new ArgumentNullException(nameof(declaringType));

            if (typeId is null)
                throw new ArgumentNullException(nameof(typeId));

            if (typeId.DeclaringType is null)
                throw new InconsistentModelException($"Cannot initialize nested type for type '{typeId}' because it has no declaring type");

            if (!typeId.DeclaringType.Equals(declaringType.TypeId))
                throw new InconsistentModelException($"Mismatch between id of type '{typeId}' and id of declaring type '{declaringType.TypeId}'");

            Assembly = assembly;
            Namespace = declaringType.Namespace;
            TypeId = typeId;
            DeclaringType = declaringType;
        }


        internal void AddNestedType(_TypeDocumentation nestedType)
        {
            if (nestedType is null)
                throw new ArgumentNullException(nameof(nestedType));

            if (!ReferenceEquals(nestedType.DeclaringType, this))
                throw new InconsistentModelException("Cannot add nested type with a different declaring type");

            m_NestedTypes.Add(nestedType);
        }
    }
}
