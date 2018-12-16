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
        private readonly XmlDocsNameMapper m_NameMapper = new XmlDocsNameMapper();

        public XmlDocsProvider(string path)
        {
            var nuDoqModel = DocReader.Read(path, new ReaderOptions() { KeepNewLinesInText = true });

            var model = ModelConverter.ConvertModel(nuDoqModel);

            m_Members = model.ToDictionary(m => m.Id);
        }


        public MemberElement TryGetDocumentationComments(TypeReference type)
        {
            var id = m_NameMapper.GetXmlDocName(type);
            return m_Members.GetValueOrDefault(id);
        }
    
        public MemberElement TryGetDocumentationComments(MethodDefinition method)
        {
            var id = m_NameMapper.GetXmlDocName(method);
            return m_Members.GetValueOrDefault(id);
        }

        public MemberElement TryGetDocumentationComments(FieldDefinition field)
        {
            var id = m_NameMapper.GetXmlDocName(field);
            return m_Members.GetValueOrDefault(id);
        }

        public MemberElement TryGetDocumentationComments(PropertyDefinition property)
        {
            var id = m_NameMapper.GetXmlDocName(property);
            return m_Members.GetValueOrDefault(id);
        }

        public MemberElement TryGetDocumentationComments(EventDefinition ev)
        {
            var id = m_NameMapper.GetXmlDocName(ev);
            return m_Members.GetValueOrDefault(id);
        }
    }
}
