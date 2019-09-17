using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    public class CommandLinePageFactory
    {
        private readonly ApplicationDocumentationBase m_Model;
        private readonly IPathProvider m_PathProvider;
        private readonly ILogger m_Logger;

        private readonly DocumentSet<IDocument> m_DocumentSet = new DocumentSet<IDocument>();


        public CommandLinePageFactory(ApplicationDocumentationBase model, IPathProvider pathProvider, ILogger logger)
        {
            m_Model = model ?? throw new ArgumentNullException(nameof(model));
            m_PathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public DocumentSet<IDocument> GetPages()
        {
            RegisterApplicationPage();

            if(m_Model is ApplicationDocumentation multiCommandApplication)
            {
                foreach (var command in multiCommandApplication.Commands)
                {
                    RegisterCommandPage(command);
                }
            }

            return m_DocumentSet;
        }


        private void RegisterApplicationPage()
        {
            IDocument page;   
            if (m_Model is ApplicationDocumentation multiCommandApplication)
            {
                m_DocumentSet.Add(
                   m_PathProvider.GetPath(multiCommandApplication),
                   new ApplicationPage(m_DocumentSet, m_PathProvider, multiCommandApplication));
            }
            else if(m_Model is SingleCommandApplicationDocumentation singleCommandApplication)
            {
                m_DocumentSet.Add(
                    m_PathProvider.GetPath(singleCommandApplication),
                    new SingleCommandApplicationPage(m_DocumentSet, m_PathProvider, singleCommandApplication));
            }
            else
            {
                throw new NotImplementedException($"Unexpected model type '{m_Model.GetType().FullName}'");
            }
        }

        private void RegisterCommandPage(CommandDocumentation command) =>
            m_DocumentSet.Add(m_PathProvider.GetPath(command), new CommandPage(command));
    }
}
