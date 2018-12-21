using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using NuDoq;

namespace MdDoc.Model.XmlDocs
{
    class XmlDocsProvider : IXmlDocsProvider
    {
        private readonly IReadOnlyDictionary<MemberId, MemberElement> m_Members;        


        public XmlDocsProvider(string xmlDocsPath, AssemblyDefinition assembly)
        {
            var nuDoqModel = DocReader.Read(xmlDocsPath);

            var model = ModelConverter.ConvertModel(nuDoqModel);

            m_Members = model.ToDictionary(m => m.MemberId);
        }


       public MemberElement TryGetDocumentationComments(MemberId id) => m_Members.GetValueOrDefault(id);
    }
}
