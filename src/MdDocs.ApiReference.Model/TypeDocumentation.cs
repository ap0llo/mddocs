using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.Utilities.Collections;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Documentation model of a type.
    /// </summary>
    public sealed class TypeDocumentation : IDocumentation, IObsoleteableDocumentation
    {
        private readonly IXmlDocsProvider m_XmlDocsProvider;
        private readonly ILogger m_Logger;
        private readonly IDictionary<MemberId, FieldDocumentation> m_Fields;
        private readonly IDictionary<MemberId, EventDocumentation> m_Events;
        private readonly IDictionary<MemberId, PropertyDocumentation> m_Properties;
        private readonly IDictionary<string, IndexerDocumentation> m_Indexers;
        private readonly IDictionary<string, MethodDocumentation> m_Methods;
        private readonly IDictionary<OperatorKind, OperatorDocumentation> m_Operators;
        private readonly List<TypeDocumentation> m_NestedTypes = new List<TypeDocumentation>();


        /// <summary>
        /// Gets the id of the type.
        /// </summary>
        public MemberId MemberId => TypeId;

        /// <summary>
        /// Gets the id of the type.
        /// </summary>
        public TypeId TypeId { get; }

        /// <summary>
        /// Gets the documentation model of the module that defines this type.
        /// </summary>
        public ModuleDocumentation ModuleDocumentation { get; }

        /// <summary>
        /// Gets the documentation model of this type's namespace.
        /// </summary>
        public NamespaceDocumentation NamespaceDocumentation { get; }

        /// <summary>
        /// Gets the type's display name
        /// </summary>
        public string DisplayName => TypeId.DisplayName;

        /// <summary>
        /// Gets the name of the assembly this type is defined in.
        /// </summary>
        public string AssemblyName => Definition.Module.Assembly.Name.Name;

        /// <summary>
        /// Gets the kind of the type (class, struct, interface ...)
        /// </summary>
        public TypeKind Kind { get; }        

        /// <summary>
        /// Gets the type's fields.
        /// </summary>
        public IReadOnlyCollection<FieldDocumentation> Fields { get; }

        /// <summary>
        /// Gets the type's events.
        /// </summary>
        public IReadOnlyCollection<EventDocumentation> Events { get; }

        /// <summary>
        /// Gets the type's properties.
        /// </summary>
        public IReadOnlyCollection<PropertyDocumentation> Properties { get; }

        /// <summary>
        /// Gets the type's indexers.
        /// </summary>
        public IReadOnlyCollection<IndexerDocumentation> Indexers { get; }

        /// <summary>
        /// Gets the type's constructors.
        /// </summary>
        public ConstructorDocumentation? Constructors { get; }

        /// <summary>
        /// Gets the type's methods.
        /// </summary>
        public IReadOnlyCollection<MethodDocumentation> Methods { get; }

        /// <summary>
        /// Gets the type's operator overloads.
        /// </summary>
        public IReadOnlyCollection<OperatorDocumentation> Operators { get; }

        /// <summary>
        /// Gets the type's base types.
        /// </summary>
        public IReadOnlyCollection<TypeId> InheritanceHierarchy { get; }

        /// <summary>
        /// Gets the interfaces this type implements.
        /// </summary>
        public IReadOnlyCollection<TypeId> ImplementedInterfaces { get; }

        /// <summary>
        /// Gets the type's custom attributes.
        /// </summary>
        public IReadOnlyCollection<TypeId> Attributes { get; }

        /// <summary>
        /// Gets the type's <c>summary</c> documentation.
        /// </summary>
        public TextBlock Summary { get; }

        /// <summary>
        /// Gets the type's <c>remarks</c> documentation.
        /// </summary>
        public TextBlock Remarks { get; }

        /// <summary>
        /// Gets the type's <c>seealso</c> documentation items.
        /// </summary>
        public IReadOnlyCollection<SeeAlsoElement> SeeAlso { get; }

        /// <summary>
        /// Gets the type's generic type parameters.
        /// </summary>
        public IReadOnlyCollection<TypeParameterDocumentation> TypeParameters { get; }

        /// <summary>
        /// Gets the definition of the type as C# code.
        /// </summary>
        public string CSharpDefinition { get; }

        /// <summary>
        /// Gets the type's <c>example</c> documentation items.
        /// </summary>
        public TextBlock Example { get; }

        /// <inheritdoc />
        public bool IsObsolete { get; }

        /// <inheritdoc />
        public string? ObsoleteMessage { get; }

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
        public TypeDocumentation? DeclaringType { get; }

        /// <summary>
        /// Gets the type's nested types
        /// </summary>
        public IReadOnlyCollection<TypeDocumentation> NestedTypes => m_NestedTypes;

        /// <summary>
        /// Gets the type's underlying Mono.Cecil definition.
        /// </summary>
        internal TypeDefinition Definition { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="TypeDocumentation"/>.
        /// </summary>
        /// <param name="moduleDocumentation">The documentation model of the module that defines the type.</param>
        /// <param name="namespaceDocumentation">The documentation model of the type's namespace.</param>
        /// <param name="definition">The type's underlying Mono.Cecil definition.</param>
        /// <param name="xmlDocsProvider">The XML documentation provider to use for loading XML documentation comments.</param>
        /// <param name="logger">The logger to use.</param>
        internal TypeDocumentation(ModuleDocumentation moduleDocumentation,
                                   NamespaceDocumentation namespaceDocumentation,
                                   TypeDefinition definition,
                                   IXmlDocsProvider xmlDocsProvider,
                                   ILogger logger,
                                   TypeDocumentation? declaringType)
        {
            TypeId = definition.ToTypeId();
            DeclaringType = declaringType;            

            ModuleDocumentation = moduleDocumentation ?? throw new ArgumentNullException(nameof(moduleDocumentation));
            NamespaceDocumentation = namespaceDocumentation ?? throw new ArgumentNullException(nameof(namespaceDocumentation));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            m_XmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            m_Logger.LogDebug($"Loading documentation for type '{definition.FullName}'");

            Kind = definition.Kind();

            m_Logger.LogDebug("Loading fields");
            m_Fields = definition.Fields
                .Where(field => field.IsPublic && !field.Attributes.HasFlag(FieldAttributes.SpecialName))
                .Select(field => new FieldDocumentation(this, field, xmlDocsProvider))
                .ToDictionary(f => f.MemberId);

            Fields = ReadOnlyCollectionAdapter.Create(m_Fields.Values);

            m_Logger.LogDebug("Loading events");
            m_Events = definition.Events
                .Where(ev => (ev.AddMethod?.IsPublic == true || ev.RemoveMethod?.IsPublic == true))
                .Select(e => new EventDocumentation(this, e, xmlDocsProvider))
                .ToDictionary(e => e.MemberId);

            Events = ReadOnlyCollectionAdapter.Create(m_Events.Values);

            m_Logger.LogDebug("Loading properties");
            m_Properties = definition.Properties
                .Where(property => (property.GetMethod?.IsPublic == true || property.SetMethod?.IsPublic == true) && !property.HasParameters)
                .Select(p => new PropertyDocumentation(this, p, xmlDocsProvider))
                .ToDictionary(p => p.MemberId);

            Properties = ReadOnlyCollectionAdapter.Create(m_Properties.Values);

            m_Logger.LogDebug("Loading indexers");
            m_Indexers = definition.Properties
                .Where(property => (property.GetMethod?.IsPublic == true || property.SetMethod?.IsPublic == true) && property.HasParameters)
                .GroupBy(p => p.Name)
                .Select(group => new IndexerDocumentation(this, group, xmlDocsProvider))
                .ToDictionary(indexer => indexer.Name);

            Indexers = ReadOnlyCollectionAdapter.Create(m_Indexers.Values);

            m_Logger.LogDebug("Loading constructors");
            var ctors = definition.GetPublicConstrutors();
            if (ctors.Any())
                Constructors = new ConstructorDocumentation(this, ctors, xmlDocsProvider);

            m_Logger.LogDebug("Loading methods");
            m_Methods = definition.GetPublicMethods()
                .Where(m => !m.IsOperator())
                .GroupBy(x => x.Name)
                .Select(group => new MethodDocumentation(this, group, xmlDocsProvider))
                .ToDictionary(m => m.Name);

            Methods = ReadOnlyCollectionAdapter.Create(m_Methods.Values);

            m_Logger.LogDebug("Loading operator overloads");
            m_Operators = definition.GetPublicMethods()
               .GroupBy(x => x.GetOperatorKind())
               .Where(group => group.Key.HasValue)
               .Select(group => new OperatorDocumentation(this, group, xmlDocsProvider))
               .ToDictionary(x => x.Kind);


            Operators = ReadOnlyCollectionAdapter.Create(m_Operators.Values);

            m_Logger.LogDebug("Loading inheritance hierarchy.");
            InheritanceHierarchy = LoadInheritanceHierarchy();

            m_Logger.LogDebug("Loading custom attributes");
            Attributes = Definition
                .GetCustomAttributes()
                .Select(x => x.AttributeType.ToTypeId())
                .ToArray();

            m_Logger.LogDebug("Loading implemented interfaces");
            ImplementedInterfaces = LoadImplementedInterfaces();

            TypeParameters = LoadTypeParameters();

            var documentationComments = m_XmlDocsProvider.TryGetDocumentationComments(MemberId);
            Summary = documentationComments?.Summary ?? TextBlock.Empty;
            Remarks = documentationComments?.Remarks ?? TextBlock.Empty;
            SeeAlso = documentationComments?.SeeAlso?.AsReadOnlyList() ?? Array.Empty<SeeAlsoElement>();
            Example = documentationComments?.Example ?? TextBlock.Empty;

            CSharpDefinition = CSharpDefinitionFormatter.GetDefinition(definition);

            IsObsolete = definition.IsObsolete(out var obsoleteMessage);
            ObsoleteMessage = obsoleteMessage;
        }


        /// <inheritdoc />
        public IDocumentation? TryGetDocumentation(MemberId id)
        {
            switch (id)
            {
                case TypeId typeId when typeId.Equals(TypeId):
                    return this;

                case FieldId fieldId when fieldId.DefiningType.Equals(TypeId):
                    return m_Fields.GetValueOrDefault(fieldId);

                case EventId eventId when eventId.DefiningType.Equals(TypeId):
                    return m_Events.GetValueOrDefault(eventId);

                case PropertyId propertyId when propertyId.DefiningType.Equals(TypeId) && propertyId.Parameters.Count == 0:
                    return m_Properties.GetValueOrDefault(propertyId);

                case PropertyId propertyId when propertyId.DefiningType.Equals(TypeId) && propertyId.Parameters.Count > 0:
                    return m_Indexers.GetValueOrDefault(propertyId.Name);

                case MethodId methodId when methodId.DefiningType.Equals(TypeId):
                    if (methodId.IsConstructor())
                    {
                        return Constructors!.TryGetDocumentation(methodId);
                    }

                    if (m_Methods.ContainsKey(methodId.Name))
                    {
                        return m_Methods[methodId.Name].TryGetDocumentation(methodId);
                    }

                    var operatorKind = methodId.GetOperatorKind();
                    if (operatorKind.HasValue && m_Operators.ContainsKey(operatorKind.Value))
                    {
                        return m_Operators[operatorKind.Value].TryGetDocumentation(methodId);
                    }

                    return null;

                default:
                    return ModuleDocumentation.TryGetDocumentation(id);
            }
        }


        internal void AddNestedType(TypeDocumentation nestedType)
        {
            if (nestedType is null)
                throw new ArgumentNullException(nameof(nestedType));

            if (nestedType.DeclaringType != this)
                throw new ArgumentException("Cannot add nested type with a different declaring type", nameof(nestedType));

            m_NestedTypes.Add(nestedType);
        }


        private IReadOnlyCollection<TypeId> LoadInheritanceHierarchy()
        {
            if (Kind == TypeKind.Interface)
                return Array.Empty<TypeId>();

            var inheritance = new LinkedList<TypeId>();
            inheritance.AddFirst(TypeId);

            TypeDefinition? currentBaseType = Definition.BaseType.Resolve();
            while (currentBaseType != null)
            {
                inheritance.AddFirst(currentBaseType.ToTypeId());
                currentBaseType = currentBaseType.BaseType?.Resolve();
            }

            return inheritance;
        }

        private IReadOnlyCollection<TypeId> LoadImplementedInterfaces()
        {
            if (!Definition.HasInterfaces)
                return Array.Empty<TypeId>();
            else
                return Definition.Interfaces.Select(x => x.InterfaceType.ToTypeId()).ToArray();
        }

        private IReadOnlyCollection<TypeParameterDocumentation> LoadTypeParameters()
        {
            if (!Definition.HasGenericParameters)
                return Array.Empty<TypeParameterDocumentation>();
            else
                return Definition.GenericParameters
                    .Select(p => new TypeParameterDocumentation(this, MemberId, p, m_XmlDocsProvider))
                    .ToArray();

        }
    }
}
