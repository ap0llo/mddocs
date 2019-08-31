using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    public class CommandLinePageFactory
    {
        private readonly ApplicationDocumentation m_Model;
        private readonly IPathProvider m_PathProvider;
        private readonly ILogger m_Logger;

        private readonly DocumentSet<IDocument> m_DocumentSet = new DocumentSet<IDocument>();


        public CommandLinePageFactory(ApplicationDocumentation model, IPathProvider pathProvider, ILogger logger)
        {
            m_Model = model ?? throw new ArgumentNullException(nameof(model));
            m_PathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public DocumentSet<IDocument> GetPages()
        {
            RegisterApplicationPage();
            foreach (var command in m_Model.Commands)
            {
                RegisterCommandPage(command);
            }

            return m_DocumentSet;
        }


        private void RegisterApplicationPage() =>
            m_DocumentSet.Add(m_PathProvider.GetPath(m_Model), new ApplicationPage(m_DocumentSet, m_PathProvider, m_Model));

        private void RegisterCommandPage(CommandDocumentation command) =>
            m_DocumentSet.Add(m_PathProvider.GetPath(command), new CommandPage(command));
    }
}
