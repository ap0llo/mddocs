using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    public sealed class OptionDocumentation : ParameterDocumentation
    {
        public override string Name { get; }

        public char? ShortName { get; }


        public override bool HasName => !String.IsNullOrEmpty(Name);

        public override bool HasShortName => ShortName != null;


        public OptionDocumentation(
            string name = null, char? shortName = null, bool required = false, string helpText = null,
            bool hidden = false, object @default = null, string metaValue = null, IReadOnlyList<string> acceptedValues = null)
            : base(required: required, helpText: helpText, hidden: hidden, @default: @default, metaValue: metaValue, acceptedValues: acceptedValues)
        {
            Name = name;
            ShortName = shortName;
        }

        private OptionDocumentation(PropertyDefinition property, ILogger logger) : base(property, property.GetAttribute(Constants.OptionAttributeFullName))
        {
            foreach (var arg in property.GetAttribute(Constants.OptionAttributeFullName).ConstructorArguments)
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
                    logger.LogWarning($"{property.FullName}: Unexpected constructor argument of type '{arg.Type.FullName}' in OptionAttribute.");
                }
            }
        }


        public static OptionDocumentation FromPropertyDefinition(PropertyDefinition definition, ILogger logger) =>
            new OptionDocumentation(definition, logger);
    }
}
