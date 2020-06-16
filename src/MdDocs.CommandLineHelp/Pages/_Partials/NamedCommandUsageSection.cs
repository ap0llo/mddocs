using System;
using System.Text;
using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    /// <summary>
    /// A partial to render the usage (textual representation of the command and the parameters) of a named command
    /// Renders
    /// <list type="bullet">
    ///     <item>A "Usage" heading</item>
    ///     <item>A code block containing the usage for the command.</item>
    /// </list>
    /// </summary>
    internal class NamedCommandUsageSection : CommandUsageSection
    {
        private readonly CommandDocumentation m_Command;


        public NamedCommandUsageSection(CommandDocumentation command) : base(command)
        {
            m_Command = command ?? throw new ArgumentNullException(nameof(command));
        }



        protected override string GetUsage()
        {
            var stringBuilder = new StringBuilder()
                .Append(m_Command.Application.Name)
                .Append(" ")
                .Append(m_Command.Name)
                .Append(" ");

            AppendParameters(stringBuilder, stringBuilder.Length);

            return stringBuilder.ToString().Trim();
        }
    }
}
