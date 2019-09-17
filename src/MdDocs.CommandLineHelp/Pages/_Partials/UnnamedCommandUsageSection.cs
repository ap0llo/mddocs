using System;
using System.Linq;
using System.Text;
using Grynwald.MarkdownGenerator;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.Common.Pages;

namespace Grynwald.MdDocs.CommandLineHelp.Pages
{
    //TODO: Share duplicate code with CommandUsageSection
    internal class UnnamedCommandUsageSection : MdPartial
    {
        private readonly UnnamedCommandDocumentation m_Command;


        public UnnamedCommandUsageSection(UnnamedCommandDocumentation command)
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


        private string GetUsage()
        {
            var stringBuilder = new StringBuilder()
                .Append(m_Command.Application.Name)
                .Append(" ");

            var prefixLength = stringBuilder.Length;

            foreach (var value in m_Command.Values.OrderBy(x => x.Index))
            {
                stringBuilder
                    .Apply(AppendUsage, value)
                    .Append(' ', prefixLength);
            }

            foreach (var option in m_Command.Options)
            {
                stringBuilder
                    .Apply(AppendUsage, option)
                    .Append(' ', prefixLength);
            }


            return stringBuilder.ToString().Trim();
        }

        private void AppendUsage(StringBuilder stringBuilder, ValueDocumentation value)
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

        private void AppendUsage(StringBuilder stringBuilder, OptionDocumentation option)
        {
            stringBuilder
                .AppendIf(!option.Required, "[")
                .Apply(AppendParameterName, option)
                .Append(" ")
                .Append("<")
                .Append(option.MetaValue ?? "VALUE")
                .Append(">")
                .AppendIf(!option.Required, "]")
                .AppendLine();
        }

        private void AppendParameterName(StringBuilder stringBuilder, OptionDocumentation option)
        {
            stringBuilder
                .AppendIf(!String.IsNullOrEmpty(option.Name), "--", option.Name)
                .AppendIf(!String.IsNullOrEmpty(option.Name) && option.ShortName != null, '|')
                .AppendIf(option.ShortName != null, "-")
                .AppendIf(option.ShortName != null, option.ShortName.GetValueOrDefault());
        }
    }
}
