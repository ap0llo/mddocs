using System;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    internal class CommandParametersSection : MdPartial
    {
        private readonly CommandDocumentation m_Command;


        public CommandParametersSection(CommandDocumentation command)
        {
            m_Command = command ?? throw new ArgumentNullException(nameof(command));
        }


        protected override MdBlock ConvertToBlock()
        {
            var detailSections = Enumerable.Concat(
                m_Command.Values.Select(v => new ValueDetailsSection(v)).Cast<ParameterDetailsSection>(),
                m_Command.Options.Select(o => new OptionDetailsSection(o)).Cast<ParameterDetailsSection>()
            ).ToArray();


            var anchors = detailSections.ToDictionary(x => x.Parameter, x => x.Heading.Anchor);

            var block = new MdContainerBlock()
            {
                new MdHeading(2, "Parameters"),
                new CommandParametersTable(m_Command, option => anchors[option], value => anchors[value])
            };

            var first = true;
            foreach (var section in detailSections)
            {
                if (detailSections.Length > 1 && !first)
                {
                    block.Add(new MdThematicBreak());
                }

                block.Add(section);
                first = false;
            }

            return block;
        }
    }
}
