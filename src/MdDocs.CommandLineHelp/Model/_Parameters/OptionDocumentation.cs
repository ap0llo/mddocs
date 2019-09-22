using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Represents a named parameter
    /// </summary>
    public sealed class OptionDocumentation : ParameterDocumentation
    {
        private readonly ILogger m_Logger;


        public override string Name { get; }

        public char? ShortName { get; }

        public override bool HasName => !String.IsNullOrEmpty(Name);

        public bool HasShortName => ShortName != null;


        public OptionDocumentation(
            string name = null, char? shortName = null, bool required = false, string helpText = null,
            bool hidden = false, object @default = null, string metaValue = null, IReadOnlyList<string> acceptedValues = null)
            : base(required: required, helpText: helpText, hidden: hidden, @default: @default, metaValue: metaValue, acceptedValues: acceptedValues)
        {
            Name = name;
            ShortName = shortName;
        }

        private OptionDocumentation(PropertyDefinition definition, ILogger logger)
            : base(definition, definition.GetAttribute(Constants.OptionAttributeFullName))
        {
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            (Name, ShortName) = LoadNames(definition);
        }


        public static OptionDocumentation FromPropertyDefinition(PropertyDefinition definition, ILogger logger) =>
            new OptionDocumentation(definition, logger);


        private (string name, char? shortName) LoadNames(PropertyDefinition definition)
        {
            string name = default;
            char? shortName = default;
            foreach (var arg in definition.GetAttribute(Constants.OptionAttributeFullName).ConstructorArguments)
            {
                if (arg.Type.FullName == typeof(string).FullName)
                {
                    name = (string)arg.Value;
                }
                else if (arg.Type.FullName == typeof(char).FullName)
                {
                    shortName = (char)arg.Value;
                }
                else
                {
                    m_Logger.LogWarning($"{definition.FullName}: Unexpected constructor argument of type '{arg.Type.FullName}' in OptionAttribute.");
                }
            }

            return (name, shortName);
        }
    }
}
