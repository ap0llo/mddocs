using System.Collections.Generic;
using System.Linq;
using Grynwald.Utilities.Collections;
using Microsoft.Extensions.Logging;

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
        /// <param name="xmlDocsPath">The path of the XML documentation file to read.</param>
        /// <param name="logger">The logger to log events to.</param>
        internal XmlDocsProvider(string xmlDocsPath, ILogger logger)
        {
            var xmlDocsReader = new XmlDocsReader(logger);
            var model = xmlDocsReader.Read(xmlDocsPath);
            m_Members = model.ToDictionary(m => m.MemberId);
        }

        /// <inheritdoc />
        public MemberElement TryGetDocumentationComments(MemberId id) => m_Members.GetValueOrDefault(id);
    }
}
