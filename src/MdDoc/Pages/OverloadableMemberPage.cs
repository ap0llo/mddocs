using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    //TODO: Use a different layout if there is only a single overloads for this operator
    abstract class OverloadableMemberPage<TModel, TOverload> : MemberPage<TModel>
        where TModel : OverloadableMemberDocumentation<TOverload>
        where TOverload : OverloadDocumentation
    {
        public OverloadableMemberPage(PageFactory pageFactory, string rootOutputPath) : base(pageFactory, rootOutputPath)
        {
        }


        public override void Save()
        {
            var document = Document(
                GetHeading()
            );

            AddDeclaringTypeSection(document.Root);

            //TODO: Attributes

            var orderedOverloads = Model.Overloads.OrderBy(x => x.Signature).ToArray();

            AddOverloadsTableSection(document.Root, orderedOverloads);

            AddOverloadDetailsSections(document.Root, orderedOverloads);

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }


        protected abstract MdHeading GetHeading();


        protected void AddOverloadsTableSection(MdContainerBlock block, IEnumerable<TOverload> methods)
        {
            var table = Table(Row("Signature", "Description"));
            foreach (var method in methods)
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

        protected void AddOverloadDetailsSections(MdContainerBlock block, IEnumerable<TOverload> methods)
        {
            foreach (var method in methods)
            {
                AddOverloadSection(block, method);
            }
        }

        protected virtual void AddOverloadSection(MdContainerBlock block, TOverload overload)
        {
            block.Add(
                Heading(overload.Signature, 2)
            );

            //TODO: Summary

            block.Add(CodeBlock(overload.CSharpDefinition, "csharp"));

            if (overload.Parameters.Count > 0)
            {
                var table = Table(Row("Name", "Type", "Description"));
                foreach (var parameter in overload.Parameters)
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
