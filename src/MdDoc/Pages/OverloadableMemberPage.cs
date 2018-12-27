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

            AddDefinitionSubSection(block, overload);

            AddParametersSubSection(block, overload);

            AddRemarksSubSection(block, overload);

            //TODO: Returns (methods) / value (indexers)
            //TODO: Exceptions
            //TODO: Remarks
            //TODO: Examples

            AddSeeAlsoSubSection(block, overload);
        }

        private void AddDefinitionSubSection(MdContainerBlock block, TOverload overload)
        {
            if(overload.Summary != null)
            {
                block.Add(TextBlockToMarkdownConverter.ConvertToBlock(overload.Summary, this));
            }

            block.Add(CodeBlock(overload.CSharpDefinition, "csharp"));
        }

        private void AddParametersSubSection(MdContainerBlock block, TOverload overload)
        {
            if (overload.Parameters.Count == 0)
                return;

            var table = Table(Row("Name", "Type", "Description"));
            foreach (var parameter in overload.Parameters)
            {
                //TODO: Description
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

        private void AddRemarksSubSection(MdContainerBlock block, TOverload overload)
        {
            if (overload.Remarks == null)
                return;

            block.Add(Heading(3, "Remarks"));
            block.Add(TextBlockToMarkdownConverter.ConvertToBlock(overload.Remarks, this));
        }

        private void AddSeeAlsoSubSection(MdContainerBlock block, TOverload overload)
        {
            if (overload.SeeAlso.Count == 0)
                return;

            block.Add(Heading(2, "See Also"));
            block.Add(
                BulletList(
                    overload.SeeAlso.Select(seeAlso => ListItem(ConvertToSpan(seeAlso)))
            ));
        }
    }
}
