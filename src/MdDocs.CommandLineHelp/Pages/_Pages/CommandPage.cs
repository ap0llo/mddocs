using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Configuration;
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
    public class CommandPage : IMarkdownDocument
    {
        private readonly DocumentSet<IDocument> m_DocumentSet;
        private readonly ICommandLineHelpPathProvider m_PathProvider;
        private readonly CommandDocumentation m_Command;
        private readonly CommandLineHelpConfiguration m_Conifguration;

        public CommandPage(DocumentSet<IDocument> documentSet, ICommandLineHelpPathProvider pathProvider, CommandDocumentation model, CommandLineHelpConfiguration configuration)
        {
            m_DocumentSet = documentSet ?? throw new ArgumentNullException(nameof(documentSet));
            m_PathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
            m_Command = model ?? throw new ArgumentNullException(nameof(model));
            m_Conifguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        public void Save(string path) => GetDocument().Save(path);

        public void Save(string path, MdSerializationOptions markdownOptions) => GetDocument().Save(path, markdownOptions);


        internal MdDocument GetDocument()
        {
            return new MdDocument()
                // Heading
                .Add(new MdHeading(1, new MdCompositeSpan(new MdCodeSpan(m_Command.Name), " Command")))
                // add application info
                .Add(GetApplicationInfo())
                // Help text
                .AddIf(!String.IsNullOrEmpty(m_Command.HelpText), () => new MdParagraph(m_Command.HelpText))
                // Usage
                .Add(new NamedCommandUsageSection(m_Command))
                // Parameters
                .AddIf(m_Command.Parameters.Count > 0, () => new CommandParametersSection(m_Command))
                // Footer
                .Add(new PageFooter());
        }


        private MdBlock GetApplicationInfo()
        {
            var applicationPage = m_DocumentSet[m_PathProvider.GetPath(m_Command.Application)];
            var link = m_DocumentSet.GetLink(this, applicationPage, m_Command.Application.Name);

            var span = new MdCompositeSpan()
            {
                new MdStrongEmphasisSpan("Application:"),
                " ",
                link
            };

            if (m_Conifguration.IncludeVersion && !String.IsNullOrEmpty(m_Command.Application.Version))
            {
                span.Add(new MdRawMarkdownSpan("\r\n"));
                span.Add(new MdStrongEmphasisSpan("Version:"));
                span.Add(" ");
                span.Add(m_Command.Application.Version);
            }

            return new MdParagraph(span);
        }
    }
}
