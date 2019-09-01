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
            var block = new MdContainerBlock()
            {
                new MdHeading(2, "Parameters"),
                new CommandParametersTable(m_Command, option => GetHeading(option).Anchor, value => GetHeading(value).Anchor)
            };

            var sections = Enumerable.Concat(
                m_Command.Values.Select(GetParameterSection),
                m_Command.Options.Select(GetParameterSection)
            ).ToArray();


            var first = true;
            foreach (var section in sections)
            {
                if (sections.Length > 1 && !first)
                {
                    block.Add(new MdThematicBreak());
                }

                block.Add(section);
                first = false;
            }

            return block;
        }


        private MdContainerBlock GetParameterSection(OptionDocumentation option)
        {
            return new MdContainerBlock()
                .AddIf(true, GetHeading(option))
                .AddIf(!String.IsNullOrEmpty(option.HelpText), () => new MdParagraph(option.HelpText))
                .AddIf(option.Default != null, GetDefaultValueParagraph, option);
        }

        private MdContainerBlock GetParameterSection(ValueDocumentation value)
        {
            return new MdContainerBlock()
                .AddIf(true, GetHeading(value))
                .AddIf(!String.IsNullOrEmpty(value.HelpText), () => new MdParagraph(value.HelpText))
                .AddIf(value.Default != null, GetDefaultValueParagraph, value);
        }

        private static MdHeading GetHeading(OptionDocumentation option)
        {
            var name = option.Name ?? option.ShortName.ToString();
            return new MdHeading(3, new MdCompositeSpan(new MdCodeSpan(name), " Parameter"));
        }

        private static MdHeading GetHeading(ValueDocumentation value)
        {
            if (!String.IsNullOrEmpty(value.Name))
            {
                return new MdHeading(3, new MdCompositeSpan(new MdCodeSpan(value.Name), $" Parameter (Position {value.Index})"));
            }
            else
            {
                //TODO: Find a better heading
                return new MdHeading(3, new MdCompositeSpan($"Parameter (Position {value.Index})"));
            }
        }

        private static MdParagraph GetDefaultValueParagraph(OptionDocumentation option)
        {
            return new MdParagraph(
                new MdStrongEmphasisSpan("Default value:"),
                " ",
                new MdCodeSpan(Convert.ToString(option.Default))
            );
        }

        private static MdParagraph GetDefaultValueParagraph(ValueDocumentation value)
        {
            return new MdParagraph(
                new MdStrongEmphasisSpan("Default value:"),
                " ",
                new MdCodeSpan(Convert.ToString(value.Default))
            );
        }
    }
}
