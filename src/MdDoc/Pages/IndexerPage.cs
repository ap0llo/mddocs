using System;
using System.IO;
using Grynwald.MarkdownGenerator;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    class IndexerPage : OverloadableMemberPage<IndexerDocumentation, IndexerOverloadDocumentation>
    {
        public override OutputPath OutputPath { get; }

        protected override IndexerDocumentation Model { get; }


        public IndexerPage(PageFactory pageFactory, string rootOutputPath, IndexerDocumentation model) : base(pageFactory, rootOutputPath)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            OutputPath = new OutputPath(Path.Combine(GetTypeDir(Model.TypeDocumentation), "indexers", $"{Model.TypeDocumentation.TypeId.Name}.{Model.Name}.md"));
        }

        protected override MdHeading GetHeading() =>
            Heading($"{Model.TypeDocumentation.DisplayName}.{Model.Name} Indexer", 1);
    }
}
