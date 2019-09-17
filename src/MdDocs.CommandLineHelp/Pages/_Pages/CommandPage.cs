using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.Common.Pages;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    /// <summary>
    /// Page that renders documentation for a single command of a application with multiple sub-commands.
    /// Shows
    /// <list type="bullet">
    ///     <item>Command name</item>
    ///     <item>Command help text</item>
    ///     <item>Usage of the command (see <see cref="NamedCommandUsageSection"/>).</item>
    ///     <item>Information about the command's parameters (see <see cref="CommandParametersSection"/>).</item>
    /// </list>
    /// </summary>
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
                .Add(new NamedCommandUsageSection(m_Command))
                // Parameters
                .AddIf(m_Command.Parameters.Count > 0, () => new CommandParametersSection(m_Command))
                // Footer
                .Add(new PageFooter());
        }
    }
}
