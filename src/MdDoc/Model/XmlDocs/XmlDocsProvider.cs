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

        private readonly IReadOnlyDictionary<MemberId, MemberElement> m_Members;        

        public XmlDocsProvider(string xmlDocsPath, AssemblyDefinition assembly)
        {
            var nuDoqModel = DocReader.Read(xmlDocsPath, new ReaderOptions() { KeepNewLinesInText = true });

            var model = ModelConverter.ConvertModel(nuDoqModel);

            m_Members = model.ToDictionary(m => m.MemberId);
        }


       public MemberElement TryGetDocumentationComments(MemberId id) => m_Members.GetValueOrDefault(id);

      
    }
}
