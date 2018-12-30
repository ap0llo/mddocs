using System;
using System.Collections.Generic;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public sealed class TypeParameterDocumentation : IDocumentation
    {
        private readonly IDocumentation m_Parent;
        private readonly GenericParameter m_Definition;


        public string Name => m_Definition.Name;

        /// <summary>
        /// Gets the type parameter's description 
        /// </summary>
        /// <value>The documentation as provided by the user or <c>null</c> if no description was specified.</value>
        public TextBlock Description { get; }


        internal TypeParameterDocumentation(IDocumentation parent, MemberId parentId, GenericParameter definition, IXmlDocsProvider xmlDocsProvider)
        {
            m_Parent = parent;
            parentId = parentId ?? throw new ArgumentNullException(nameof(parentId));
            m_Definition = definition;
            xmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));

            Description = xmlDocsProvider.TryGetDocumentationComments(parentId)?.TypeParameters?.GetValueOrDefault(definition.Name);
        }


        public IDocumentation TryGetDocumentation(MemberId member) => m_Parent.TryGetDocumentation(member);
    }
}
