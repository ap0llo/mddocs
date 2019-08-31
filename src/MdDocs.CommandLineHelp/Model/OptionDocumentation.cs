using System.Linq;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    public sealed class OptionDocumentation : ParameterDocumentation
    {
        public string Name { get; }

        public char? ShortName { get; }


        public OptionDocumentation(string name = null, char? shortName = null, string helpText = null, bool hidden = false, object @default = null)
            : base(helpText: helpText, hidden: hidden, @default: @default)
        {
            Name = name;
            ShortName = shortName;
        }

        private OptionDocumentation(PropertyDefinition definition) : base(definition.GetAttribute(Constants.OptionAttributeFullName))
        {
            var optionAttribute = definition.GetAttribute(Constants.OptionAttributeFullName);

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

            Name = name;
            ShortName = shortName;
        }


        public static OptionDocumentation FromPropertyDefinition(PropertyDefinition definition, ILogger logger) =>
            new OptionDocumentation(definition);
    }
}
