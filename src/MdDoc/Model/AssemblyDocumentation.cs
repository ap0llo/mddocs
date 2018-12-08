﻿using MdDoc.XmlDocs;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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


        public static AssemblyDocumentation FromFile(string filePath)
        {
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(filePath);

            var docsFilePath = Path.ChangeExtension(filePath, ".xml");

            var xmlDocsProvider = File.Exists(docsFilePath)
                ? (IXmlDocsProvider) new XmlDocsProvider(docsFilePath)
                : (IXmlDocsProvider) new NullXmlDocsProvider();

            return new AssemblyDocumentation(assemblyDefinition, xmlDocsProvider);
        }

        public TypeDocumentation TryGetDocumentation(TypeName type) => 
            MainModuleDocumentation.TryGetDocumentation(type);
    }
}
