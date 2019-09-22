using System;
using System.Linq;
using System.Text;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.Common.Pages;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    /// <summary>
    /// A partial to render the usage (textual representation of the command and the parameters) of a unnamed command
    /// Renders
    /// <list type="bullet">
    ///     <item>A "Usage" heading</item>
    ///     <item>A code block containing the usage for the command.</item>
    /// </list>
    /// </summary>
    internal class UnnamedCommandUsageSection : CommandUsageSection
    {
        private readonly UnnamedCommandDocumentation m_Command;


        public UnnamedCommandUsageSection(UnnamedCommandDocumentation command) : base(command)
        {
            m_Command = command ?? throw new ArgumentNullException(nameof(command));
        }

        protected override string GetUsage()
        {
            var stringBuilder = new StringBuilder()
                .Append(m_Command.Application.Name)
                .Append(" ");

            AppendParameters(stringBuilder, stringBuilder.Length);

            return stringBuilder.ToString().Trim();
        }

    }
}
