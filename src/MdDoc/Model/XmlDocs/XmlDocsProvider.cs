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

        private readonly IReadOnlyDictionary<MemberReference, MemberElement> m_Members;        

        public XmlDocsProvider(string xmlDocsPath, AssemblyDefinition assembly)
        {
            var nuDoqModel = DocReader.Read(xmlDocsPath, new ReaderOptions() { KeepNewLinesInText = true });

            var model = ModelConverter.ConvertModel(nuDoqModel, GetAssemblyMembers(assembly));

            m_Members = model.ToDictionary(m => m.Reference);
        }


        public MemberElement TryGetDocumentationComments(TypeReference type) => m_Members.GetValueOrDefault(type);

        public MemberElement TryGetDocumentationComments(MethodDefinition method) => m_Members.GetValueOrDefault(method);

        public MemberElement TryGetDocumentationComments(FieldReference field) => m_Members.GetValueOrDefault(field);

        public MemberElement TryGetDocumentationComments(PropertyReference property) => m_Members.GetValueOrDefault(property);

        public MemberElement TryGetDocumentationComments(EventReference ev) => m_Members.GetValueOrDefault(ev);


        private IReadOnlyDictionary<string, MemberReference> GetAssemblyMembers(AssemblyDefinition assembly)
        {
            var assemblyMembersById = new Dictionary<string, MemberReference>();
            foreach(var type in assembly.MainModule.GetTypes())
            {
                assemblyMembersById.Add(type.GetXmlDocId(), type);

                foreach(var method in type.Methods)
                {
                    assemblyMembersById.Add(method.GetXmlDocId(), method);
                }

                foreach(var field in type.Fields)
                {
                    assemblyMembersById.Add(field.GetXmlDocId(), field);
                }

                foreach(var property in type.Properties)
                {
                    assemblyMembersById.Add(property.GetXmlDocId(), property);
                }

                foreach(var ev in type.Events)
                {
                    assemblyMembersById.Add(ev.GetXmlDocId(), ev);
                }
            }

            return assemblyMembersById;   
        }
    }
}
