using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.Common.Pages;
using Grynwald.Utilities.Collections;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal abstract class OverloadableMemberPage<TModel, TOverload> : MemberPage<TModel>
        where TModel : OverloadableMemberDocumentation<TOverload>
        where TOverload : OverloadDocumentation
    {
        private readonly ILogger m_Logger;
        private readonly Lazy<IReadOnlyDictionary<MemberId, MdHeading>> m_Headings;


        internal OverloadableMemberPage(PageFactory pageFactory, string rootOutputPath, TModel model, ILogger logger)
            : base(pageFactory, rootOutputPath, model)
        {
            m_Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            m_Headings = new Lazy<IReadOnlyDictionary<MemberId, MdHeading>>(LoadHeadings);
        }


        public override void Save() => Save(OutputPath);

        public override void Save(string path)
        {
            m_Logger.LogInformation($"Saving page '{path}'");

            var document = new MdDocument(
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
                    document.Root.Add(m_Headings.Value[overload.MemberId]);
                    AddOverloadSection(document.Root, overload, 2);
                }
            }

            document.Root.Add(new PageFooter());
            document.Save(path);
        }


        public override bool TryGetAnchor(MemberId id, out string anchor)
        {
            if (m_Headings.Value.ContainsKey(id))
            {
                anchor = m_Headings.Value[id].Anchor;
                return true;
            }
            else
            {
                anchor = default;
                return false;
            }
        }

        protected abstract MdHeading GetPageHeading();

        protected void AddOverloadsTableSection(MdContainerBlock block, IEnumerable<TOverload> overloads, int headingLevel)
        {
            var table = new MdTable(new MdTableRow("Signature", "Description"));
            foreach (var overload in overloads)
            {
                // optimization: we know the section we're linking to is on the same page
                // so we can create the link to the anchor without going through PageBase.CreateLink()
                var link = new MdLinkSpan(overload.Signature, "#" + m_Headings.Value[overload.MemberId].Anchor);
                table.Add(
                    new MdTableRow(link, ConvertToSpan(overload.Summary))
                );
            }

            block.Add(
                new MdHeading("Overloads", headingLevel),
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

            block.Add(new MdCodeBlock(overload.CSharpDefinition, "csharp"));
        }

        protected virtual void AddTypeParametersSubSection(MdContainerBlock block, TOverload overload, int headingLevel)
        {
            if (overload.TypeParameters.Count == 0)
                return;


            block.Add(new MdHeading("Type Parameters", headingLevel));

            foreach (var typeParameter in overload.TypeParameters)
            {
                block.Add(
                    new MdParagraph(new MdCodeSpan(typeParameter.Name)
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


            parametersBlock.Add(new MdHeading("Parameters", headingLevel));

            foreach (var parameter in overload.Parameters)
            {
                parametersBlock.Add(
                    new MdParagraph(
                        new MdCodeSpan(parameter.Name),
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

            block.Add(new MdHeading("Returns", headingLevel));

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

            block.Add(new MdHeading("Exceptions", headingLevel));

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

            block.Add(new MdHeading("Example", headingLevel));
            block.Add(ConvertToBlock(overload.Example));
        }

        protected virtual void AddRemarksSubSection(MdContainerBlock block, TOverload overload, int headingLevel)
        {
            if (overload.Remarks == null)
                return;

            block.Add(new MdHeading("Remarks", headingLevel));
            block.Add(ConvertToBlock(overload.Remarks));
        }

        protected virtual void AddSeeAlsoSubSection(MdContainerBlock block, TOverload overload, int headingLevel)
        {
            if (overload.SeeAlso.Count == 0)
                return;

            block.Add(new MdHeading("See Also", headingLevel));
            block.Add(
                new MdBulletList(
                    overload.SeeAlso.Select(seeAlso => new MdListItem(ConvertToSpan(seeAlso)))
            ));
        }


        private IReadOnlyDictionary<MemberId, MdHeading> LoadHeadings()
        {
            if (Model.Overloads.Count == 1)
            {
                return ReadOnlyDictionary<MemberId, MdHeading>.Empty;
            }
            else
            {
                var headings = new Dictionary<MemberId, MdHeading>();
                foreach (var overload in Model.Overloads)
                {
                    headings.Add(overload.MemberId, new MdHeading(overload.Signature, 2));
                }
                return headings;
            }
        }
    }
}
