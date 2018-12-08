using Mono.Cecil;
using NuDoq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.XmlDocs
{
    class XmlDocsProvider : IXmlDocsProvider
    {
        class DocumentationLoaderVisitor : Visitor
        {
            private Member m_CurrentMember;
            private Dictionary<string, Summary> m_Summaries = new Dictionary<string, Summary>();

            public IReadOnlyDictionary<string, Summary> Summaries => m_Summaries;


            public override void VisitType(TypeDeclaration type)
            {
                m_CurrentMember = type;
                base.VisitType(type);
                m_CurrentMember = null;
            }

            public override void VisitMethod(Method method)
            {
                m_CurrentMember = method;
                base.VisitMethod(method);
                m_CurrentMember = null;
            }

            public override void VisitField(Field field)
            {
                m_CurrentMember = field;
                base.VisitField(field);
                m_CurrentMember = null;
            }

            public override void VisitProperty(Property property)
            {
                m_CurrentMember = property;
                base.VisitProperty(property);
                m_CurrentMember = null;
            }

            public override void VisitEvent(Event ev)
            {
                m_CurrentMember = ev;
                base.VisitEvent(ev);
                m_CurrentMember = null;
            }

            public override void VisitSummary(Summary summary)
            {
                if(m_CurrentMember != null)
                {
                    m_Summaries.Add(m_CurrentMember.Id, summary);
                }

                // do not visit any further down into the hierary
            }            
        }


        private readonly IReadOnlyDictionary<string, Summary> m_Summaries;
        private readonly XmlDocsNameMapper m_NameMapper = new XmlDocsNameMapper();

        public XmlDocsProvider(string path)
        {
            var members = DocReader.Read(path, new ReaderOptions() { KeepNewLinesInText = true });

            var visitor = new DocumentationLoaderVisitor();
            members.Accept(visitor);

            m_Summaries = visitor.Summaries;
        }


        public Summary TryGetSummary(TypeReference type)
        {
            var id = m_NameMapper.GetXmlDocName(type);
            return m_Summaries.GetValueOrDefault(id);
        }
    
        public Summary TryGetSummary(MethodDefinition method)
        {
            var id = m_NameMapper.GetXmlDocName(method);
            return m_Summaries.GetValueOrDefault(id);
        }

        public Summary TryGetSummary(FieldDefinition field)
        {
            var id = m_NameMapper.GetXmlDocName(field);
            return m_Summaries.GetValueOrDefault(id);
        }

        public Summary TryGetSummary(PropertyDefinition property)
        {
            var id = m_NameMapper.GetXmlDocName(property);
            return m_Summaries.GetValueOrDefault(id);
        }

        public Summary TryGetSummary(EventDefinition ev)
        {
            var id = m_NameMapper.GetXmlDocName(ev);
            return m_Summaries.GetValueOrDefault(id);
        }
    }
}
