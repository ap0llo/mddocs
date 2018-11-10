using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MdDoc.Model;
using Mono.Cecil;
using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc
{
    class MethodPage : MemberPage
    {
        private readonly MethodDocumentation m_Model;       


        protected override TypeReference DeclaringType => m_Model.Overloads.First().DeclaringType;

        protected override OutputPath OutputPath { get; }


        public MethodPage(DocumentationContext context, PathProvider pathProvider, MethodDocumentation model) 
            : base(context, pathProvider)
        {
            m_Model = model ?? throw new ArgumentNullException(nameof(model));
            OutputPath = m_PathProvider.GetMethodOutputPath(model.Overloads.First());
        }


        public override void Save()
        {
            var document = Document(
                Heading($"{m_Model.Name}.{m_Model.Name} Method", 1)
            );

            AddDeclaringTypeSection(document.Root);            

            AddOverloadsSection(document.Root, m_Model.Overloads);

            AddDetailSections(document.Root, m_Model.Overloads);

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }
    }
}
