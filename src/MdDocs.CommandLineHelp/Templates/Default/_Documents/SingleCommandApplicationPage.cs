﻿using System;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Configuration;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.Common;
using Grynwald.MdDocs.Common.Pages;

namespace Grynwald.MdDocs.CommandLineHelp.Templates.Default
{
    /// <summary>
    /// Page that renders documentation for a application without sub-commands
    /// Shows
    /// <list type="bullet">
    ///     <item>Application name</item>
    ///     <item>Application version</item>
    ///     <item>Application usage (if the assembly has a AssemblyUsage attribute)</item>
    ///     <item>Command help text</item>
    ///     <item>Usage of the command (see <see cref="SingleCommandApplicationUsageSection"/>).</item>
    ///     <item>Information about the command's parameters (see <see cref="CommandParametersSection"/>).</item>
    /// </list>
    /// </summary>
    public class SingleCommandApplicationPage : IMarkdownDocument
    {
        private readonly SingleCommandApplicationDocumentation m_Model;
        private readonly CommandLineHelpConfiguration m_Configuration;
        private readonly DocumentSet<IDocument> m_DocumentSet;
        private readonly ICommandLineHelpPathProvider m_PathProvider;


        public SingleCommandApplicationPage(
            DocumentSet<IDocument> documentSet,
            ICommandLineHelpPathProvider pathProvider,
            SingleCommandApplicationDocumentation model,
            CommandLineHelpConfiguration configuration)
        {
            m_DocumentSet = documentSet ?? throw new ArgumentNullException(nameof(documentSet));
            m_PathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
            m_Model = model ?? throw new ArgumentNullException(nameof(model));
            m_Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        public void Save(string path) => GetDocument().Save(path);

        public void Save(string path, MdSerializationOptions markdownOptions) => GetDocument().Save(path, markdownOptions);


        internal MdDocument GetDocument()
        {
            return new MdDocument()
                .AddIf(m_Configuration.Template.Default.IncludeAutoGeneratedNotice, new AutoGeneratedNotice())

                // Application name and version
                .Add(new MdHeading(1, $"{m_Model.Name} Command Line Reference"))
                .AddIf(m_Configuration.Template.Default.IncludeVersion, new ApplicationVersionBlock(m_Model))

                // Usage (data from ApplicationUsage attribute)
                .AddIf(m_Model.Usage != null && m_Model.Usage.Count > 0, new MdHeading(2, "Usage"))
                .AddIf(m_Model.Usage != null && m_Model.Usage.Count > 0, m_Model.Usage != null ? (MdBlock)new MdParagraph(String.Join(Environment.NewLine, m_Model.Usage!)) : MdEmptyBlock.Instance)

                // Usage / example call
                .Add(new SingleCommandApplicationUsageSection(m_Model))

                // Parameters
                .AddIf(m_Model.AllParameters.Any(), () => new CommandParametersSection(m_Model))

                // Footer
                .Add(new PageFooter());
        }
    }
}
