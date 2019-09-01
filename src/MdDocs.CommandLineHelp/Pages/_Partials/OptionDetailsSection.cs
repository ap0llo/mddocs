using System;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    internal class OptionDetailsSection : ParameterDetailsSection
    {
        private readonly OptionDocumentation m_Option;


        public override MdHeading Heading { get; }

        public override ParameterDocumentation Parameter => m_Option;


        public OptionDetailsSection(OptionDocumentation option)
        {
            m_Option = option ?? throw new ArgumentNullException(nameof(option));

            var name = option.Name ?? option.ShortName.ToString();
            Heading = new MdHeading(3, new MdCompositeSpan(new MdCodeSpan(name), " Parameter"));
        }



        protected override MdBlock ConvertToBlock()
        {
            return new MdContainerBlock()
                .AddIf(true, Heading)
                .AddIf(!String.IsNullOrEmpty(m_Option.HelpText), () => new MdParagraph(m_Option.HelpText))
                .AddIf(true, GetDetailsTable());
        }


        private MdTable GetDetailsTable()
        {
            // TODO: Type

            var table = new MdTable(new MdTableRow("", ""));

            if(m_Option.HasName)
            {
                table.Add(new MdTableRow("Name:", m_Option.Name));
            }

            if(m_Option.HasShortName)
            {
                if(!m_Option.HasName)
                    table.Add(new MdTableRow("Name:", m_Option.ShortName.ToString()));
                else
                    table.Add(new MdTableRow("Short name:", m_Option.ShortName.ToString()));
            }

            table.Add(new MdTableRow("Position:", new MdEmphasisSpan("Named parameter")));

            table.Add(new MdTableRow("Required:", m_Option.Required ? "Yes" : "No"));

            //TODO: Add allowed values for enum types
            if(m_Option.HasMetaValue)
            {
                table.Add(new MdTableRow("Value:", m_Option.MetaValue));
            }


            if (m_Option.HasDefault)
            {
                table.Add(new MdTableRow("Default value:", Convert.ToString(m_Option.Default)));
            }
            else
            {
                table.Add(new MdTableRow("Default value:", new MdEmphasisSpan("None")));
            }
            return table;
        }
    }
}
