using MdDoc.Model.XmlDocs;
using Mono.Cecil;
using NuDoq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Model.XmlDocs
{
    class XmlDocsProvider : IXmlDocsProvider
    {

        private readonly IReadOnlyDictionary<string, MemberElement> m_Members;        

        public XmlDocsProvider(string path)
        {
            var nuDoqModel = DocReader.Read(path, new ReaderOptions() { KeepNewLinesInText = true });

            var model = ModelConverter.ConvertModel(nuDoqModel);

            m_Members = model.ToDictionary(m => m.Id);
        }


        public MemberElement TryGetDocumentationComments(TypeReference type)
        {
            var id = type.GetXmlDocId();
            return m_Members.GetValueOrDefault(id);
        }
    
        public MemberElement TryGetDocumentationComments(MethodDefinition method)
        {
            var id = method.GetXmlDocId();
            return m_Members.GetValueOrDefault(id);
        }

        public MemberElement TryGetDocumentationComments(FieldReference field)
        {
            var id = field.GetXmlDocId();
            return m_Members.GetValueOrDefault(id);
        }

        public MemberElement TryGetDocumentationComments(PropertyReference property)
        {
            var id = property.GetXmlDocId();
            return m_Members.GetValueOrDefault(id);
        }

        public MemberElement TryGetDocumentationComments(EventReference ev)
        {
            var id = ev.GetXmlDocId();
            return m_Members.GetValueOrDefault(id);
        }
    }
}
