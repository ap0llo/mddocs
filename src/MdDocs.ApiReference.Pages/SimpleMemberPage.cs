using System;
using System.IO;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.Common.Pages;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.ApiReference.Pages
{
    public abstract class SimpleMemberPage<TModel> : MemberPage<TModel> where TModel : SimpleMemberDocumentation
    {
        private readonly ILogger m_Logger;


        internal SimpleMemberPage(ILinkProvider linkProvider, TModel model, ILogger logger)
            : base(linkProvider, model)
        {
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void Save(string path)
        {
            m_Logger.LogInformation($"Saving page '{path}'");

            var document = new MdDocument(
                GetHeading()
            );

            AddObsoleteWarning(document.Root, Model);

            AddDeclaringTypeSection(document.Root);

            AddDefinitionSection(document.Root);

            AddValueSection(document.Root);

            AddRemarksSection(document.Root);

            AddExampleSection(document.Root);

            AddSeeAlsoSection(document.Root);

            document.Root.Add(new PageFooter());
            
            document.Save(path);
        }


        protected virtual void AddDefinitionSection(MdContainerBlock block)
        {
            if (Model.Summary != null)
            {
                block.Add(ConvertToBlock(Model.Summary));
            }

            block.Add(
                new MdCodeBlock(Model.CSharpDefinition, "csharp")
            );
        }

        protected virtual void AddRemarksSection(MdContainerBlock block)
        {
            if (Model.Remarks == null)
                return;

            block.Add(new MdHeading(2, "Remarks"));
            block.Add(ConvertToBlock(Model.Remarks));
        }

        protected virtual void AddExampleSection(MdContainerBlock block)
        {
            if (Model.Example == null)
                return;

            block.Add(new MdHeading(2, "Example"));
            block.Add(ConvertToBlock(Model.Example));
        }

        protected virtual void AddSeeAlsoSection(MdContainerBlock block)
        {
            if (Model.SeeAlso.Count > 0)
            {
                block.Add(new MdHeading(2, "See Also"));
                block.Add(
                    new MdBulletList(
                        Model.SeeAlso.Select(seeAlso => new MdListItem(ConvertToSpan(seeAlso)))
                ));
            }
        }


        protected abstract MdHeading GetHeading();

        protected abstract void AddValueSection(MdContainerBlock block);
    }
}
