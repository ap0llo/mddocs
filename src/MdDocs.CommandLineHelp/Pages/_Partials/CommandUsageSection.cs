using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.Common.Pages;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    internal abstract class CommandUsageSection : MdPartial
    {
        private readonly CommandDocumentationBase m_Command;

        public CommandUsageSection(CommandDocumentationBase command)
        {
            m_Command = command ?? throw new ArgumentNullException(nameof(command));
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
            foreach (var value in m_Command.Values.OrderBy(x => x.Index))
            {
                stringBuilder
                    .Apply(AppendUsage, value)
                    .Append(' ', indent);
            }

            foreach (var option in m_Command.Options)
            {
                stringBuilder
                    .Apply(AppendUsage, option)
                    .Append(' ', indent);
            }
        }

        protected void AppendUsage(StringBuilder stringBuilder, ValueDocumentation value)
        {
            stringBuilder
                .AppendIf(!value.Required, "[")
                .Append("<")
                .Append(value.Name ?? "VALUE")
                .AppendIf(!String.IsNullOrEmpty(value.MetaValue), ":", value.MetaValue)
                .Append(">")
                .AppendIf(!value.Required, "]")
                .AppendLine();
        }

        protected void AppendUsage(StringBuilder stringBuilder, OptionDocumentation option)
        {
            stringBuilder
                .AppendIf(!option.Required, "[")
                .Apply(AppendParameterName, option);

            // omit value for switch parameters
            if (!option.IsSwitchParameter)
            {
                stringBuilder
                    .Append(" ")
                    .Append("<")
                    .Append(option.MetaValue ?? "VALUE")
                    .Append(">");
            }

            stringBuilder
                .AppendIf(!option.Required, "]")
                .AppendLine();
        }

        protected void AppendParameterName(StringBuilder stringBuilder, OptionDocumentation option)
        {
            stringBuilder
                .AppendIf(!String.IsNullOrEmpty(option.Name), "--", option.Name)
                .AppendIf(!String.IsNullOrEmpty(option.Name) && option.ShortName != null, '|')
                .AppendIf(option.ShortName != null, "-")
                .AppendIf(option.ShortName != null, option.ShortName.GetValueOrDefault());
        }
    }
}
