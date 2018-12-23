using System;
using System.Collections.Generic;
using System.IO;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    //TODO: Use a different layout if there is only a single overloads for this operator
    class OperatorPage : MemberPage<OperatorDocumentation>
    {
        public override OutputPath OutputPath { get; }
            
        protected override OperatorDocumentation Model { get; }


        public OperatorPage(PageFactory pageFactory, string rootOutputPath, OperatorDocumentation model) 
            : base(pageFactory, rootOutputPath)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));   
            OutputPath = new OutputPath(Path.Combine(GetTypeDir(Model.TypeDocumentation), "operators", $"{Model.TypeDocumentation.TypeId.Name}.{Model.Kind}.md"));
        }


        public override void Save()
        {
            var document = Document(
                Heading($"{Model.TypeDocumentation.DisplayName}.{Model.Kind} Operator", 1)
            );

            AddDeclaringTypeSection(document.Root);

            //TODO: Summary

            AddOverloadsSection(document.Root, Model.Overloads);

            AddDetailSections(document.Root, Model.Overloads);

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }


        //TODO: Reuse code from MethodPage
        protected void AddOverloadsSection(MdContainerBlock block, IEnumerable<OperatorOverloadDocumentation> overloads)
        {
            var table = Table(Row("Signature", "Description"));
            foreach (var method in overloads)
            {
                table.Add(
                    Row(method.Signature, ConvertToSpan(method.Summary))
                );
            }

            block.Add(
                Heading("Overloads", 2),
                table
            );
        }

        protected void AddDetailSections(MdContainerBlock block, IEnumerable<OperatorOverloadDocumentation> overloads)
        {
            foreach (var method in overloads)
            {
                block.Add(
                    Heading(method.Signature, 2)
                );

                //TODO: Summary

                block.Add(CodeBlock(method.CSharpDefinition, "csharp"));

                if (method.Parameters.Count > 0)
                {
                    var table = Table(Row("Name", "Type", "Description"));
                    foreach (var parameter in method.Parameters)
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

                //TODO: Returns
                //TODO: Exceptions
                //TODO: Remarks
                //TODO: Examples

            }
        }
    }
}
