using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Microsoft.Extensions.Logging;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    public class CommandLinePageFactory
    {
        private readonly ApplicationDocumentation m_Model;
        private readonly ILogger m_Logger;


        public CommandLinePageFactory(ApplicationDocumentation model, ILogger logger)
        {
            m_Model = model ?? throw new ArgumentNullException(nameof(model));
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public DocumentSet<MdDocument> GetPages()
        {
            var documentSet = new DocumentSet<MdDocument>();

            foreach (var command in m_Model.Commands)
            {
                var page = new CommandPage(command);
                documentSet.Add($"commands/{command.Name}.md", page.GetDocument());
            }

            return documentSet;
        }
    }
}
