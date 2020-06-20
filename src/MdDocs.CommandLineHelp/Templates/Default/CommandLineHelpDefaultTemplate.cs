using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Configuration;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.Common.Templates;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.CommandLineHelp.Templates.Default
{
    public class CommandLineHelpDefaultTemplate : ITemplate<ApplicationDocumentation>
    {
        private readonly CommandLineHelpConfiguration m_Configuration;
        private readonly ICommandLineHelpPathProvider m_PathProvider;
        private readonly ILogger m_Logger;

        private readonly DocumentSet<IDocument> m_DocumentSet = new DocumentSet<IDocument>();


        public CommandLineHelpDefaultTemplate(CommandLineHelpConfiguration configuration, ICommandLineHelpPathProvider pathProvider, ILogger logger)
        {
            m_Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            m_PathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <inheritdoc />
        public DocumentSet<IDocument> Render(ApplicationDocumentation model)
        {
            RegisterApplicationPage(model);

            if (model is MultiCommandApplicationDocumentation multiCommandApplication)
            {
                foreach (var command in multiCommandApplication.Commands)
                {
                    RegisterCommandPage(command);
                }
            }

            return m_DocumentSet;
        }


        private void RegisterApplicationPage(ApplicationDocumentation model)
        {
            if (model is MultiCommandApplicationDocumentation multiCommandApplication)
            {
                m_DocumentSet.Add(
                   m_PathProvider.GetPath(multiCommandApplication),
                   new MultiCommandApplicationPage(m_DocumentSet, m_PathProvider, multiCommandApplication, m_Configuration));
            }
            else if (model is SingleCommandApplicationDocumentation singleCommandApplication)
            {
                m_DocumentSet.Add(
                    m_PathProvider.GetPath(singleCommandApplication),
                    new SingleCommandApplicationPage(m_DocumentSet, m_PathProvider, singleCommandApplication, m_Configuration));
            }
            else
            {
                throw new NotImplementedException($"Unexpected model type '{model.GetType().FullName}'");
            }
        }

        private void RegisterCommandPage(CommandDocumentation command) =>
            m_DocumentSet.Add(m_PathProvider.GetPath(command), new CommandPage(m_DocumentSet, m_PathProvider, command, m_Configuration));
    }
}
