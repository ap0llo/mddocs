using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace MdDoc.Model.XmlDocs
{
    class NullXmlDocsProvider : IXmlDocsProvider
    {
        public MemberElement TryGetDocumentationComments(MemberId id) => null;
    }
}
