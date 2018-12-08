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

        public Summary TryGetSummary(FieldDefinition method) => default;

        public Summary TryGetSummary(PropertyDefinition property) => default;

        public Summary TryGetSummary(EventDefinition ev) => default;
    }
}
