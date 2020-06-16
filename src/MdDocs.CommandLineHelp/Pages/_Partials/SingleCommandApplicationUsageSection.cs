using System;
using System.Text;
using Grynwald.MdDocs.CommandLineHelp.Model;

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
    internal class SingleCommandApplicationUsageSection : CommandUsageSection
    {
        private readonly SingleCommandApplicationDocumentation m_Model;


        public SingleCommandApplicationUsageSection(SingleCommandApplicationDocumentation command) : base(command)
        {
            m_Model = command ?? throw new ArgumentNullException(nameof(command));
        }

        protected override string GetUsage()
        {
            var stringBuilder = new StringBuilder()
                .Append(m_Model.Name)
                .Append(" ");

            AppendParameters(stringBuilder, stringBuilder.Length);

            return stringBuilder.ToString().Trim();
        }

    }
}
