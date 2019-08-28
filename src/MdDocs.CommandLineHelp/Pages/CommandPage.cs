using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    internal class CommandPage
    {
        public static MdDocument GetDocument(CommandDocumentation model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            var document = new MdDocument();

            document.Root.Add(new MdHeading(1, new MdCompositeSpan(new MdCodeSpan(model.Name), " Command")));

            if (!String.IsNullOrEmpty(model.HelpText))
            {
                document.Root.Add(new MdParagraph(model.HelpText));
            }

            return document;
        }
    }
}
