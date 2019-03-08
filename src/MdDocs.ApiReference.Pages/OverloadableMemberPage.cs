using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    //TODO: Use a different layout if there is only a single overloads for this operator
    internal abstract class OverloadableMemberPage<TModel, TOverload> : MemberPage<TModel>
        where TModel : OverloadableMemberDocumentation<TOverload>
        where TOverload : OverloadDocumentation
    {
        public OverloadableMemberPage(PageFactory pageFactory, string rootOutputPath, TModel model)
            : base(pageFactory, rootOutputPath, model)
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

            AddObsoleteWarning(block, overload);

            AddDefinitionSubSection(block, overload);

            AddTypeParametersSubSection(block, overload);

            AddParametersSubSection(block, overload);

            AddRemarksSubSection(block, overload);

            AddReturnsSubSection(block, overload);

            AddExceptionsSubSection(block, overload);

            AddExampleSubSection(block, overload);

            AddSeeAlsoSubSection(block, overload);
        }


        protected virtual void AddDefinitionSubSection(MdContainerBlock block, TOverload overload)
        {
            if (overload.Summary != null)
            {
                block.Add(ConvertToBlock(overload.Summary));
            }

            block.Add(CodeBlock(overload.CSharpDefinition, "csharp"));
        }

        protected virtual void AddTypeParametersSubSection(MdContainerBlock block, TOverload overload)
        {
            if (overload.TypeParameters.Count == 0)
                return;


            block.Add(Heading("Type Parameters", 3));

            foreach (var typeParameter in overload.TypeParameters)
            {
                block.Add(
                    Paragraph(CodeSpan(typeParameter.Name)
                ));

                if (typeParameter.Description != null)
                {
                    block.Add(ConvertToBlock(typeParameter.Description));
                }
            }
        }

        protected virtual void AddParametersSubSection(MdContainerBlock block, TOverload overload)
        {
            if (overload.Parameters.Count == 0)
                return;

            var parametersBlock = new MdContainerBlock();
            block.Add(parametersBlock);


            parametersBlock.Add(Heading("Parameters", 3));

            foreach (var parameter in overload.Parameters)
            {
                parametersBlock.Add(
                    Paragraph(
                        CodeSpan(parameter.Name),
                        "  ",
                        GetMdSpan(parameter.ParameterType)
                ));

                if (parameter.Description != null)
                {
                    parametersBlock.Add(ConvertToBlock(parameter.Description));
                }
            }
        }

        protected virtual void AddReturnsSubSection(MdContainerBlock block, TOverload overload)
        {
            // skip "Returns" section for void methods
            if (overload.Type.IsVoid)
                return;

            block.Add(Heading("Returns", 3));

            // add return type
            block.Add(
                GetMdParagraph(overload.Type)
            );

            // add returns documentation
            if (overload.Returns != null)
            {
                block.Add(ConvertToBlock(overload.Returns));
            }
        }

        protected virtual void AddExceptionsSubSection(MdContainerBlock block, TOverload overload)
        {
            if (overload.Exceptions.Count == 0)
                return;

            block.Add(Heading("Exceptions", 3));

            foreach (var exception in overload.Exceptions)
            {
                block.Add(
                    GetMdParagraph(exception.Type),
                    ConvertToBlock(exception.Text)
                );
            }
        }

        protected virtual void AddExampleSubSection(MdContainerBlock block, TOverload overload)
        {
            if (overload.Example == null)
                return;

            block.Add(Heading("Example", 3));
            block.Add(ConvertToBlock(overload.Example));
        }

        protected virtual void AddRemarksSubSection(MdContainerBlock block, TOverload overload)
        {
            if (overload.Remarks == null)
                return;

            block.Add(Heading(3, "Remarks"));
            block.Add(ConvertToBlock(overload.Remarks));
        }

        protected virtual void AddSeeAlsoSubSection(MdContainerBlock block, TOverload overload)
        {
            if (overload.SeeAlso.Count == 0)
                return;

            block.Add(Heading(3, "See Also"));
            block.Add(
                BulletList(
                    overload.SeeAlso.Select(seeAlso => ListItem(ConvertToSpan(seeAlso)))
            ));
        }
    }
}
