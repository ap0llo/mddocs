using Grynwald.MarkdownGenerator;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MdDoc
{
    class DefaultXmlDocProvider : IXmlDocProvider
    {
        private readonly XmlDocNameMapper m_NameMapper = new XmlDocNameMapper();
        private readonly ModuleDefinition m_Module;
        private readonly string m_XmlDocsPath;
        public readonly Dictionary<MemberReference, MemberDocumentation> m_MemberDocumentation = new Dictionary<MemberReference, MemberDocumentation>();


        public DefaultXmlDocProvider(ModuleDefinition module, string xmlDocsPath)
        {
            m_Module = module;
            m_XmlDocsPath = xmlDocsPath;

            Load();
        }


        public MemberDocumentation TryGetDocumentation(MemberReference member)
        {
            return m_MemberDocumentation.TryGetValue(member, out var documentation) ? documentation : MemberDocumentation.Empty;
        }


        private void Load()
        {
            var namesByMemberReference = GetXmlDocMemberNames();
            var docsByName = LoadXmlDocumentation();

            // check if the xml docs contain a member we did not find
            var namesInAssembly = namesByMemberReference.Values.ToHashSet();            
            var unrecognizedNames = docsByName.Keys.Where(x => !namesInAssembly.Contains(x)).ToArray();
            foreach (var name in unrecognizedNames)
            {
                Console.WriteLine($"WARNING: Unrecignized name '{name}' in XML docs");
            }


            foreach(var memberReference in namesByMemberReference.Keys)
            {
                var name = namesByMemberReference[memberReference];

                if(docsByName.ContainsKey(name))
                {
                    m_MemberDocumentation.Add(memberReference, new MemberDocumentation(docsByName[name]));
                }
            }
        }


        private IDictionary<MemberReference, string> GetXmlDocMemberNames()
        {
            var result = new Dictionary<MemberReference, string>();

            foreach (var type in m_Module.Types)
            {
                result.Add(type, m_NameMapper.GetXmlDocName(type));

                foreach (var property in type.Properties)
                {
                    result.Add(property, m_NameMapper.GetXmlDocName(property));
                }

                foreach (var field in type.Fields)
                {
                    result.Add(field, m_NameMapper.GetXmlDocName(field));
                }

                foreach (var method in type.Methods)
                {
                    result.Add(method, m_NameMapper.GetXmlDocName(method));
                }            

                //TODO: Events
            }

            return result;
        }


        private IDictionary<string, XElement> LoadXmlDocumentation()
        {
            var document = XDocument.Load(m_XmlDocsPath);

            return document.Root
                .Descendants("member")
                .ToDictionary(x => x.Attribute("name").Value);

        }
    }
}
