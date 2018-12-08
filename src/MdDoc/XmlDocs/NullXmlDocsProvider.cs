using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using NuDoq;

namespace MdDoc.XmlDocs
{
    class NullXmlDocsProvider : IXmlDocsProvider
    {
        public Summary TryGetSummary(TypeReference type) => default;

        public Summary TryGetSummary(MethodDefinition method) => default;
    }
}
