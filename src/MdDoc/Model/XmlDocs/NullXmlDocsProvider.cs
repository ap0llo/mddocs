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

        public MemberElement TryGetDocumentationComments(FieldReference method) => default;

        public MemberElement TryGetDocumentationComments(PropertyReference property) => default;

        public MemberElement TryGetDocumentationComments(EventReference ev) => default;
    }
}
