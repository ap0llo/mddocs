﻿using System.Collections.Generic;
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

            AddTypeParametersSubSection(block, overload);

            AddParametersSubSection(block, overload);

            AddRemarksSubSection(block, overload);

            //TODO: Returns (methods) / value (indexers)
            //TODO: Exceptions            
            //TODO: Examples

            AddSeeAlsoSubSection(block, overload);
        }

        protected virtual void AddDefinitionSubSection(MdContainerBlock block, TOverload overload)
        {
            if(overload.Summary != null)
            {
                block.Add(TextBlockToMarkdownConverter.ConvertToBlock(overload.Summary, this));
            }

            block.Add(CodeBlock(overload.CSharpDefinition, "csharp"));
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
                        GetTypeNameSpan(parameter.ParameterType)
                ));

                if (parameter.Description != null)
                {
                    //TODO: Add ConvertToBlock method to PageBase to make these calls a little more readable
                    parametersBlock.Add(TextBlockToMarkdownConverter.ConvertToBlock(parameter.Description, this));
                }                
            }            
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
                    block.Add(TextBlockToMarkdownConverter.ConvertToBlock(typeParameter.Description, this));
                }
            }
        }


        protected virtual void AddRemarksSubSection(MdContainerBlock block, TOverload overload)
        {
            if (overload.Remarks == null)
                return;

            block.Add(Heading(3, "Remarks"));
            block.Add(TextBlockToMarkdownConverter.ConvertToBlock(overload.Remarks, this));
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