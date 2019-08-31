using System.Linq;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    public sealed class OptionDocumentation
    {
        public string Name { get; }

        public char? ShortName { get; }

        public string HelpText { get; }

        public bool Hidden { get; }

        public object Default { get; }


        public OptionDocumentation(string name = null, char? shortName = null, string helpText = null, bool hidden = false, object @default = null)
        {
            Name = name;
            ShortName = shortName;
            HelpText = helpText;
            Hidden = hidden;
            Default = @default;
        }


        public static OptionDocumentation FromPropertyDefinition(PropertyDefinition definition, ILogger logger)
        {

            var optionAttribute = definition.CustomAttributes.Single(a => a.AttributeType.FullName == Constants.OptionAttributeFullName);

            string name;
            char? shortName;
            if (optionAttribute.ConstructorArguments.Count == 1)
            {
                var arg = optionAttribute.ConstructorArguments.Single();
                if (arg.Type.FullName == "System.String")
                {
                    name = (string)arg.Value;
                    shortName = null;
                }
                else
                {
                    name = null;
                    shortName = (char)arg.Value;
                }
            }
            else
            {
                name = (string)optionAttribute.ConstructorArguments.Skip(1).First().Value;
                shortName = (char)optionAttribute.ConstructorArguments.First().Value;
            }

            var helpText = optionAttribute.Properties.SingleOrDefault(p => p.Name == "HelpText").Argument.Value as string;
            var hidden = (optionAttribute.Properties.SingleOrDefault(p => p.Name == "Hidden").Argument.Value as bool?) ?? false;

            var defaultValue = optionAttribute.Properties.SingleOrDefault(p => p.Name == "Default").Argument.Value;
            // no idea why the CustomAttributeArgument.Value returns the value wrapped in

            // another CustomAttributeArgument in some cases
            if (defaultValue is CustomAttributeArgument)
            {
                defaultValue = ((CustomAttributeArgument)defaultValue).Value;
            }

            return new OptionDocumentation(
                name: name,
                shortName: shortName,
                helpText: helpText,
                hidden: hidden,
                @default: defaultValue);
        }
    }
}
