using Grynwald.MarkdownGenerator;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc
{
    class ConstructorsPage : MemberPage
    {
        private readonly TypeDefinition m_Type;

        public override string Name => $"{m_Type.Name} Constructors";

        protected override OutputPath OutputPath => m_PathProvider.GetConstructorsOutputPath(m_Type);

        protected override TypeReference DeclaringType => m_Type;

        
        public ConstructorsPage(DocumentationContext context, PathProvider pathProvider, TypeDefinition type)
            : base(context, pathProvider)
        {
            m_Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public override void Save()
        {
            var document = Document(
                Heading($"{m_Type.Name} Constructors", 1)
            );

            AddDeclaringTypeSection(document.Root);

            AddOverloadsSection(document.Root);

            AddDetailSections(document.Root);

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }


        private void AddOverloadsSection(MdContainerBlock block)
        {
            var table = Table(Row("Signature", "Description"));
            foreach(var ctor in m_Type.GetDocumentedConstrutors(m_Context))
            {
                table.Add(
                    Row(GetSignature(ctor))
                );
            }

            block.Add(
                Heading("Overloads", 2),
                table
            );
        }

        private void AddDetailSections(MdContainerBlock block)
        {
            foreach (var ctor in m_Type.GetDocumentedConstrutors(m_Context))
            {
                block.Add(
                    Heading(GetSignature(ctor), 2)
                );

                //TODO: Attributes

                if(ctor.Parameters.Any())
                {
                    var table = Table(Row("Name", "Type", "Description"));
                    foreach(var parameter in ctor.Parameters)
                    {
                        table.Add(
                            Row(
                                CodeSpan(parameter.Name),
                                GetTypeNameSpan(parameter.ParameterType),
                                ""
                        ));
                    }

                    block.Add(
                        Heading("Parameters", 3),
                        table
                    );                    
                }

            }
        }


    }
}
