using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace MdDoc.Model.XmlDocs
{
    class NullXmlDocsProvider : IXmlDocsProvider
    {
        public MemberElement TryGetDocumentationComments(TypeReference type) => default;

        public MemberElement TryGetDocumentationComments(MethodDefinition method) => default;

        public MemberElement TryGetDocumentationComments(FieldDefinition method) => default;

        public MemberElement TryGetDocumentationComments(PropertyDefinition property) => default;

        public MemberElement TryGetDocumentationComments(EventDefinition ev) => default;
    }
}
