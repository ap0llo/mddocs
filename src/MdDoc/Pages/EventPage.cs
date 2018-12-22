using System;
using System.IO;
using MdDoc.Model;

using static Grynwald.MarkdownGenerator.FactoryMethods;

namespace MdDoc.Pages
{
    class EventPage : MemberPage<EventDocumentation>
    {        
        public override OutputPath OutputPath { get; }            

        protected override EventDocumentation Model { get; }


        public EventPage(PageFactory pageFactory, string rootOutputPath, EventDocumentation model)
            : base(pageFactory, rootOutputPath)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            OutputPath = new OutputPath(Path.Combine(GetTypeDir(Model.TypeDocumentation), "events", $"{Model.TypeDocumentation.TypeId.Name}.{Model.Name}.md"));
        }


        public override void Save()
        {
            var document = Document(
                Heading($"{Model.TypeDocumentation.DisplayName}.{Model.Name} Event", 1)
            );

            AddDeclaringTypeSection(document.Root);

            //TODO: Summary

            //TODO: C# Definition           

            //TODO: Remarks

            //TODO: Examples

            //TODO: SeeAlso

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            document.Save(OutputPath);
        }



    }
}
