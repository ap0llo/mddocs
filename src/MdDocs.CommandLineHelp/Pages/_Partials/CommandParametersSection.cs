using System;
using System.Linq;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.Common.Pages;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    /// <summary>
    /// A partial to render a section with information about a command's parameters.
    /// Renders
    /// <list type="bullet">
    ///     <item>A "Parameters" heading.</item>
    ///     <item>A table containing all the commands parameters (see <see cref="CommandParametersTable"/>).</item>
    ///     <item>A details section for every parameter.</item>
    /// </list>
    /// </summary>
    internal class CommandParametersSection : MdPartial
    {
        private readonly IParameterCollection m_Model;


        public CommandParametersSection(IParameterCollection command)
        {
            m_Model = command ?? throw new ArgumentNullException(nameof(command));
        }


        protected override MdBlock ConvertToBlock()
        {
            var detailSections = Enumerable.Empty<ParameterDetailsSection>()
                .Concat(m_Model.PositionalParameters.Select(v => new PositionalParameterDetailsSection(v)).Cast<ParameterDetailsSection>())
                .Concat(m_Model.NamedParameters.Select(o => new NamedParameterDetailsSection(o)).Cast<ParameterDetailsSection>())
                .Concat(m_Model.SwitchParameters.Select(o => new SwitchParameterDetailsSection(o)).Cast<ParameterDetailsSection>())
                .ToArray();


            var anchors = detailSections.ToDictionary(x => x.Parameter, x => x.Heading.Anchor);

            var block = new MdContainerBlock()
            {
                new MdHeading(2, "Parameters"),
                new CommandParametersTable(m_Model, parameter => anchors[parameter], parameter => anchors[parameter], parameter => anchors[parameter])
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
