using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace Grynwald.MdDocs.ApiReference.Pages
{
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
                GetPageHeading()
            );

            AddDeclaringTypeSection(document.Root);

            //TODO: List method Attributes

            if (Model.Overloads.Count == 1)
            {
                AddOverloadSection(document.Root, Model.Overloads.Single(), 1);
            }
            else
            {
                var orderedOverloads = Model.Overloads.OrderBy(x => x.Signature).ToArray();

                AddOverloadsTableSection(document.Root, orderedOverloads, headingLevel: 2);

                foreach (var overload in orderedOverloads)
                {
                    document.Root.Add(Heading(overload.Signature, 2));
                    AddOverloadSection(document.Root, overload, 2);
                }
            }

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }


        protected abstract MdHeading GetPageHeading();


        protected void AddOverloadsTableSection(MdContainerBlock block, IEnumerable<TOverload> methods, int headingLevel)
        {
            var table = Table(Row("Signature", "Description"));
            foreach (var method in methods)
            {
                table.Add(
                    Row(method.Signature, ConvertToSpan(method.Summary))
                );
            }

            block.Add(
                Heading("Overloads", headingLevel),
                table
            );
        }

        protected virtual void AddOverloadSection(MdContainerBlock block, TOverload overload, int headingLevel)
        {
            AddObsoleteWarning(block, overload);

            AddDefinitionSubSection(block, overload, headingLevel + 1);

            AddTypeParametersSubSection(block, overload, headingLevel + 1);

            AddParametersSubSection(block, overload, headingLevel + 1);

            AddRemarksSubSection(block, overload, headingLevel + 1);

            AddReturnsSubSection(block, overload, headingLevel + 1);

            AddExceptionsSubSection(block, overload, headingLevel + 1);

            AddExampleSubSection(block, overload, headingLevel + 1);

            AddSeeAlsoSubSection(block, overload, headingLevel + 1);
        }

        protected virtual void AddDefinitionSubSection(MdContainerBlock block, TOverload overload, int headingLevel)
        {
            if (overload.Summary != null)
            {
                block.Add(ConvertToBlock(overload.Summary));
            }

            block.Add(CodeBlock(overload.CSharpDefinition, "csharp"));
        }

        protected virtual void AddTypeParametersSubSection(MdContainerBlock block, TOverload overload, int headingLevel)
        {
            if (overload.TypeParameters.Count == 0)
                return;


            block.Add(Heading("Type Parameters", headingLevel));

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

        protected virtual void AddParametersSubSection(MdContainerBlock block, TOverload overload, int headingLevel)
        {
            if (overload.Parameters.Count == 0)
                return;

            var parametersBlock = new MdContainerBlock();
            block.Add(parametersBlock);


            parametersBlock.Add(Heading("Parameters", headingLevel));

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

        protected virtual void AddReturnsSubSection(MdContainerBlock block, TOverload overload, int headingLevel)
        {
            // skip "Returns" section for void methods
            if (overload.Type.IsVoid)
                return;

            block.Add(Heading("Returns", headingLevel));

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

        protected virtual void AddExceptionsSubSection(MdContainerBlock block, TOverload overload, int headingLevel)
        {
            if (overload.Exceptions.Count == 0)
                return;

            block.Add(Heading("Exceptions", headingLevel));

            foreach (var exception in overload.Exceptions)
            {
                block.Add(
                    GetMdParagraph(exception.Type),
                    ConvertToBlock(exception.Text)
                );
            }
        }

        protected virtual void AddExampleSubSection(MdContainerBlock block, TOverload overload, int headingLevel)
        {
            if (overload.Example == null)
                return;

            block.Add(Heading("Example", headingLevel));
            block.Add(ConvertToBlock(overload.Example));
        }

        protected virtual void AddRemarksSubSection(MdContainerBlock block, TOverload overload, int headingLevel)
        {
            if (overload.Remarks == null)
                return;

            block.Add(Heading("Remarks", headingLevel));
            block.Add(ConvertToBlock(overload.Remarks));
        }

        protected virtual void AddSeeAlsoSubSection(MdContainerBlock block, TOverload overload, int headingLevel)
        {
            if (overload.SeeAlso.Count == 0)
                return;

            block.Add(Heading("See Also", headingLevel));
            block.Add(
                BulletList(
                    overload.SeeAlso.Select(seeAlso => ListItem(ConvertToSpan(seeAlso)))
            ));
        }
    }
}
