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

        private OptionDocumentation(PropertyDefinition definition, ILogger logger) : base(definition.GetAttribute(Constants.OptionAttributeFullName))
        {
            foreach (var arg in definition.GetAttribute(Constants.OptionAttributeFullName).ConstructorArguments)
            {
                if (arg.Type.FullName == typeof(string).FullName)
                {
                    Name = (string)arg.Value;
                }
                else if (arg.Type.FullName == typeof(char).FullName)
                {
                    ShortName = (char)arg.Value;
                }
                else
                {
                    logger.LogWarning($"{definition.FullName}: Unexpected constructor argument of type '{arg.Type.FullName}' in OptionAttribute.");
                }
            }
        }


        public static OptionDocumentation FromPropertyDefinition(PropertyDefinition definition, ILogger logger) =>
            new OptionDocumentation(definition, logger);
    }
}
