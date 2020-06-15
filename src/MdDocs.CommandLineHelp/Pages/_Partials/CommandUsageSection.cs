using System;
using System.Linq;
using System.Text;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.Common.Pages;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    internal abstract class CommandUsageSection : MdPartial
    {
        private readonly IParameterCollection m_Model;

        public CommandUsageSection(IParameterCollection model)
        {
            m_Model = model ?? throw new ArgumentNullException(nameof(model));
        }


        protected override MdBlock ConvertToBlock()
        {
            return new MdContainerBlock(
                new MdHeading(2, "Usage"),
                new MdCodeBlock(GetUsage())
            );
        }

        protected abstract string GetUsage();

        protected void AppendParameters(StringBuilder stringBuilder, int indent)
        {
            foreach (var value in m_Model.PositionalParameters.OrderBy(x => x.Position))
            {
                stringBuilder
                    .Apply(AppendUsage, value)
                    .Append(' ', indent);
            }

            foreach (var option in m_Model.NamedParameters)
            {
                stringBuilder
                    .Apply(AppendUsage, option)
                    .Append(' ', indent);
            }

            foreach (var option in m_Model.SwitchParameters)
            {
                stringBuilder
                    .Apply(AppendUsage, option)
                    .Append(' ', indent);
            }

        }

        protected void AppendUsage(StringBuilder stringBuilder, PositionalParameterDocumentation parameter)
        {
            stringBuilder
                .AppendIf(!parameter.Required, "[")
                .Append("<")
                .Append(String.IsNullOrWhiteSpace(parameter.InformationalName) ? "VALUE" : parameter.InformationalName)
                .AppendIf(!String.IsNullOrEmpty(parameter.ValuePlaceHolderName), ":", parameter.ValuePlaceHolderName)
                .Append(">")
                .AppendIf(!parameter.Required, "]")
                .AppendLine();
        }

        protected void AppendUsage(StringBuilder stringBuilder, SwitchParameterDocumentation parameter)
        {
            stringBuilder
                .Append("[")
                .Apply(AppendParameterName, parameter)
                .Append("]")
                .AppendLine();

        }

        protected void AppendUsage(StringBuilder stringBuilder, NamedValuedParameterDocumentation parameter)
        {
            stringBuilder
                .AppendIf(!parameter.Required, "[")
                .Apply(AppendParameterName, parameter);

            stringBuilder
                .Append(" ")
                .Append("<")
                .Append(String.IsNullOrWhiteSpace(parameter.ValuePlaceHolderName) ? "VALUE" : parameter.ValuePlaceHolderName)
                .Append(">");

            stringBuilder
                .AppendIf(!parameter.Required, "]")
                .AppendLine();
        }

        protected void AppendParameterName(StringBuilder stringBuilder, INamedParameterDocumentation parameter)
        {
            stringBuilder
                .AppendIf(parameter.HasName, "--", parameter.Name!)
                .AppendIf(parameter.HasName && parameter.HasShortName, '|')
                .AppendIf(parameter.HasShortName, "-")
                .AppendIf(parameter.HasShortName, parameter.ShortName!);
        }
    }
}
