using Grynwald.Utilities.Collections;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MdDoc.Model
{
    public class ModuleDocumentation : IDocumentation
    {
        private readonly IDictionary<TypeName, TypeDocumentation> m_Types;
        private readonly IXmlDocsProvider m_XmlDocsProvider;

        public AssemblyDocumentation AssemblyDocumentation { get; }

        public IReadOnlyCollection<TypeDocumentation> Types { get; }

        internal ModuleDefinition Definition { get; }


        internal ModuleDocumentation(AssemblyDocumentation assemblyDocumentation, ModuleDefinition definition, IXmlDocsProvider xmlDocsProvider)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            m_XmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));

            AssemblyDocumentation = assemblyDocumentation ?? throw new ArgumentNullException(nameof(assemblyDocumentation));

            m_Types = Definition.Types
                .Where(t => t.IsPublic)
                .ToDictionary(typeDefinition => new TypeName(typeDefinition), typeDefinition => new TypeDocumentation(this, typeDefinition, m_XmlDocsProvider));

            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);
        }


        public TypeDocumentation TryGetDocumentation(TypeName type) => m_Types.GetValueOrDefault(type);

    }
}
