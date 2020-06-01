using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Grynwald.Utilities.Collections;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    /// <summary>
    /// Default implementation of <see cref="IXmlDocsProvider"/>
    /// </summary>
    internal class XmlDocsProvider : IXmlDocsProvider
    {
        private readonly IReadOnlyDictionary<MemberId, MemberElement> m_Members;

        /// <summary>
        /// Initialize a new instance of <see cref="XmlDocsProvider"/>.
        /// </summary>
        /// <param name="assemblyDefinition">The assembly to load documentation for.</param>
        /// <param name="xmlDocs">The XML documentation to read.</param>
        /// <param name="logger">The logger to log events to.</param>
        internal XmlDocsProvider(AssemblyDefinition assemblyDefinition, XDocument xmlDocs, ILogger logger)
        {
            if (assemblyDefinition is null)
                throw new ArgumentNullException(nameof(assemblyDefinition));

            var outerTypes = assemblyDefinition
                .MainModule
                .Types
                .Select(x => x.ToTypeId())
                .ToHashSet();

            var xmlDocsReader = new XmlDocsReader(logger, xmlDocs, outerTypes);
            var model = xmlDocsReader.Read();
            m_Members = model.ToDictionary(m => m.MemberId);
        }

        /// <summary>
        /// Initialize a new instance of <see cref="XmlDocsProvider"/>.
        /// </summary>
        /// <param name="assemblyDefinition">The assembly to load documentation for.</param>
        /// <param name="xmlDocsPath">The path of the XML documentation file to read.</param>
        /// <param name="logger">The logger to log events to.</param>
        internal XmlDocsProvider(AssemblyDefinition assemblyDefinition, string xmlDocsPath, ILogger logger)
        {
            if (assemblyDefinition is null)
                throw new ArgumentNullException(nameof(assemblyDefinition));

            var outerTypes = assemblyDefinition
                .MainModule
                .Types
                .Select(x => x.ToTypeId())
                .ToHashSet();

            var xmlDocsReader = new XmlDocsReader(logger, xmlDocsPath, outerTypes);
            var model = xmlDocsReader.Read();
            m_Members = model.ToDictionary(m => m.MemberId);
        }

        /// <inheritdoc />
        public MemberElement? TryGetDocumentationComments(MemberId id) => m_Members.GetValueOrDefault(id);

    }
}
