using System;
using System.IO;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Microsoft.Extensions.Logging;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    internal abstract class SimpleMemberPage<TModel> : MemberPage<TModel> where TModel : SimpleMemberDocumentation
    {
        private readonly ILogger m_Logger;

        public SimpleMemberPage(PageFactory pageFactory, string rootOutputPath, TModel model, ILogger logger)
            : base(pageFactory, rootOutputPath, model)
        {
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public override void Save()
        {
            m_Logger.LogInformation($"Saving page '{OutputPath}'");

            var document = Document(
                GetHeading()
            );

            AddObsoleteWarning(document.Root, Model);

            AddDeclaringTypeSection(document.Root);

            AddDefinitionSection(document.Root);

            AddValueSection(document.Root);

            AddRemarksSection(document.Root);

            AddExampleSection(document.Root);

            AddSeeAlsoSection(document.Root);

            AddFooter(document.Root);

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }


        protected virtual void AddDefinitionSection(MdContainerBlock block)
        {
            if (Model.Summary != null)
            {
                block.Add(ConvertToBlock(Model.Summary));
            }

            block.Add(
                CodeBlock(Model.CSharpDefinition, "csharp")
            );
        }

        protected virtual void AddRemarksSection(MdContainerBlock block)
        {
            if (Model.Remarks == null)
                return;

            block.Add(Heading(2, "Remarks"));
            block.Add(ConvertToBlock(Model.Remarks));
        }

        protected virtual void AddExampleSection(MdContainerBlock block)
        {
            if (Model.Example == null)
                return;

            block.Add(Heading(2, "Example"));
            block.Add(ConvertToBlock(Model.Example));
        }

        protected virtual void AddSeeAlsoSection(MdContainerBlock block)
        {
            if (Model.SeeAlso.Count > 0)
            {
                block.Add(Heading(2, "See Also"));
                block.Add(
                    BulletList(
                        Model.SeeAlso.Select(seeAlso => ListItem(ConvertToSpan(seeAlso)))
                ));
            }
        }


        protected abstract MdHeading GetHeading();

        protected abstract void AddValueSection(MdContainerBlock block);
    }
}
