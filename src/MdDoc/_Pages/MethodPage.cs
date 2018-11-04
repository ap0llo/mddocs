using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;
using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc
{
    class MethodPage : MemberPage
    {
        private readonly TypeDefinition m_Type;
        private readonly string m_MethodName;

        public override string Name => $"{m_Type.Name}.{m_MethodName} Method";

        protected override TypeReference DeclaringType => m_Type;

        protected override OutputPath OutputPath { get; }


        public MethodPage(DocumentationContext context, PathProvider pathProvider, TypeDefinition type, string methodName) 
            : base(context, pathProvider)
        {
            m_Type = type ?? throw new ArgumentNullException(nameof(type));
            m_MethodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
            OutputPath = m_PathProvider.GetMethodOutputPath(type, methodName);
        }


        public override void Save()
        {
            var document = Document(
                Heading($"{m_Type.Name}.{m_MethodName} Method", 1)
            );

            AddDeclaringTypeSection(document.Root);

            var methods = m_Type.GetDocumentedMethods(m_Context).Where(x => x.Name.Equals(m_MethodName)).ToArray();

            AddOverloadsSection(document.Root, methods);

            AddDetailSections(document.Root, methods);

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }
    }
}
