using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Represents a named parameter
    /// </summary>
    public sealed class OptionDocumentation : ParameterDocumentation
    {
        private readonly ILogger m_Logger;

        /// <inheritdoc />
        public override string? Name { get; }

        /// <inheritdoc />
        public override object? Default { get; }

        public char? ShortName { get; }

        public override bool HasName => !String.IsNullOrEmpty(Name);

        public bool HasShortName => ShortName != null;

        public override bool Required { get; }

        public bool IsSwitchParameter { get; }


        internal OptionDocumentation(
            string? name = null, char? shortName = null, bool required = false, string? helpText = null,
            bool hidden = false, object? @default = null, string? metaValue = null, IReadOnlyList<string>? acceptedValues = null,
            bool isSwitchParameter = false)
            : base(required: required, helpText: helpText, hidden: hidden, @default: @default, metaValue: metaValue, acceptedValues: acceptedValues)
        {
            Name = name;
            ShortName = shortName;
            m_Logger = NullLogger.Instance;

            IsSwitchParameter = isSwitchParameter;
            Required = required;
            Default = @default;
        }

        private OptionDocumentation(PropertyDefinition definition, ILogger logger)
            : base(definition, definition.GetAttribute(Constants.OptionAttributeFullName))
        {
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            (Name, ShortName) = LoadNames(definition);

            // special handling for bool options:
            // bool options are treated as "switch" parameters that do not require a value to be passed in,
            // e.g. the option is set using "--option" not "--option true"
            if (definition.PropertyType.FullName == Constants.BooleanFullName)
            {
                IsSwitchParameter = true;

                // switch parameters are always optional
                Required = false;
                if(base.Required)
                {
                    // emit a warning if parameter was flagged as required
                    m_Logger.LogWarning($"Ignoring 'Required' flag of option '{Name}'. Boolean options are treated as switch parameter and cannot be required.");
                }

                // default for switch parameters is always false
                Default = false;
                if(base.Default != null && (!(base.Default is bool) || (base.Default is bool defaultValue && defaultValue != false)))
                {
                    // emit a warning if a default value other than 'false' was required
                    m_Logger.LogWarning($"Ignoring default value '{base.Default}' of option '{Name}'. Boolean options are treated as switch parameter with a fixed default value 'false'");
                }
            }
            else
            {
                IsSwitchParameter = false;
                Required = base.Required;
                Default = base.Default;
            }
        }


        public static OptionDocumentation FromPropertyDefinition(PropertyDefinition definition, ILogger logger) =>
            new OptionDocumentation(definition, logger);


        private (string? name, char? shortName) LoadNames(PropertyDefinition definition)
        {
            string? name = default;
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
