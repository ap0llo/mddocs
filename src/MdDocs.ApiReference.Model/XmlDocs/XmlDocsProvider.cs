using System.Collections.Generic;
using System.Linq;
using Grynwald.Utilities.Collections;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    internal class XmlDocsProvider : IXmlDocsProvider
    {
        private readonly IReadOnlyDictionary<MemberId, MemberElement> m_Members;

        internal XmlDocsProvider(string xmlDocsPath, ILogger logger)
        {
            var model = XmlDocsReader.Read(xmlDocsPath, logger);
            m_Members = model.ToDictionary(m => m.MemberId);
        }


        public MemberElement TryGetDocumentationComments(MemberId id) => m_Members.GetValueOrDefault(id);
    }
}
