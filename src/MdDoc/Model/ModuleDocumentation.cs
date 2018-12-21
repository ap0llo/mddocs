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
        private readonly IDictionary<TypeId, TypeDocumentation> m_Types;
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
                .Select(typeDefinition => new TypeDocumentation(this, typeDefinition, m_XmlDocsProvider))
                .ToDictionary(typeDocumentation => typeDocumentation.TypeId);

            Types = ReadOnlyCollectionAdapter.Create(m_Types.Values);
        }


        public IDocumentation TryGetDocumentation(MemberId member)
        {
            switch (member)
            {
                case TypeId typeId:
                    return m_Types.GetValueOrDefault(typeId);

                case TypeMemberId typeMemberId:
                    return m_Types.GetValueOrDefault(typeMemberId.DefiningType)?.TryGetDocumentation(member);

                default:
                    return null;
            }
        }
    }
}
