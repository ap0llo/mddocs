using Mono.Cecil;
using NuDoq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.XmlDocs
{
    interface IXmlDocsProvider
    {
        Summary TryGetSummary(TypeReference type);

        Summary TryGetSummary(MethodDefinition method);
    }
}
