using System;
using System.IO;
using MdDoc.Model.XmlDocs;
using Mono.Cecil;

namespace MdDoc.Model
{
    public class AssemblyDocumentation : IDisposable, IDocumentation
    {
        private readonly IXmlDocsProvider m_XmlDocsProvider;


        internal AssemblyDefinition Definition { get; }

        public ModuleDocumentation MainModuleDocumentation { get; }



        private AssemblyDocumentation(AssemblyDefinition definition, IXmlDocsProvider xmlDocsProvider)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            m_XmlDocsProvider = xmlDocsProvider ?? throw new ArgumentNullException(nameof(xmlDocsProvider));

            MainModuleDocumentation = new ModuleDocumentation(this, definition.MainModule, m_XmlDocsProvider);
        }


        public void Dispose()
        {
            Definition.Dispose();
        }

        public IDocumentation TryGetDocumentation(MemberId member) =>
            MainModuleDocumentation.TryGetDocumentation(member);

        public static AssemblyDocumentation FromFile(string filePath)
        {
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(filePath);

            var docsFilePath = Path.ChangeExtension(filePath, ".xml");

            var xmlDocsProvider = File.Exists(docsFilePath)
                ? (IXmlDocsProvider) new XmlDocsProvider(docsFilePath, assemblyDefinition)
                : (IXmlDocsProvider) NullXmlDocsProvider.Instance;

            return new AssemblyDocumentation(assemblyDefinition, xmlDocsProvider);
        }        
    }
}
