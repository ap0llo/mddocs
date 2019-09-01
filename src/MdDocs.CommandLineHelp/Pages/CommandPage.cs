using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    public class CommandPage : IDocument
    {
        private readonly CommandDocumentation m_Command;


        public CommandPage(CommandDocumentation model)
        {
            m_Command = model ?? throw new ArgumentNullException(nameof(model));
        }


        public void Save(string path) => GetDocument().Save(path);


        internal MdDocument GetDocument()
        {
            return new MdDocument()
                // Heading
                .Add(new MdHeading(1, new MdCompositeSpan(new MdCodeSpan(m_Command.Name), " Command")))
                // Help text
                .AddIf(!String.IsNullOrEmpty(m_Command.HelpText), () => new MdParagraph(m_Command.HelpText))
                // Usage
                .Add(new CommandUsageSection(m_Command))
                // Parameters
                .AddIf(m_Command.Parameters.Count > 0, () => new CommandParametersSection(m_Command));
        }
    }
}
