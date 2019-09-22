using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.Common.Pages;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    /// <summary>
    /// Page that renders documentation for a application without sub-commands
    /// Shows
    /// <list type="bullet">
    ///     <item>Application name</item>
    ///     <item>Application version</item>
    ///     <item>Application usage (if the assembly has a AssemblyUsage attribute)</item>
    ///     <item>Command help text</item>
    ///     <item>Usage of the command (see <see cref="UnnamedCommandUsageSection"/>).</item>
    ///     <item>Information about the command's parameters (see <see cref="CommandParametersSection"/>).</item>
    /// </list>
    /// </summary>
    public class SingleCommandApplicationPage : IDocument
    {
        private readonly SingleCommandApplicationDocumentation m_Model;
        private readonly DocumentSet<IDocument> m_DocumentSet;
        private readonly ICommandLineHelpPathProvider m_PathProvider;


        public SingleCommandApplicationPage(DocumentSet<IDocument> documentSet, ICommandLineHelpPathProvider pathProvider, SingleCommandApplicationDocumentation model)
        {
            m_DocumentSet = documentSet ?? throw new ArgumentNullException(nameof(documentSet));
            m_PathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
            m_Model = model ?? throw new ArgumentNullException(nameof(model));
        }


        public void Save(string path) => GetDocument().Save(path);


        internal MdDocument GetDocument()
        {
            return new MdDocument()

                // Application name and version
                .Add(new MdHeading(1, $"{m_Model.Name} Command Line Reference"))
                .Add(new ApplicationVersionBlock(m_Model))

                // Usage (data from ApplicationUsage attribute)
                .AddIf(m_Model.Usage.Count > 0, new MdHeading(2, "Usage"))
                .AddIf(m_Model.Usage.Count > 0, new MdParagraph(String.Join(Environment.NewLine, m_Model.Usage)))

                // Usage / example call
                .Add(new UnnamedCommandUsageSection(m_Model.Command))

                // Parameters
                .AddIf(m_Model.Command.Parameters.Count > 0, () => new CommandParametersSection(m_Model.Command))

                // Footer
                .Add(new PageFooter());
        }
    }
}
