using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    public sealed class CommandDocumentation
    {
        public string Name { get; }

        public string HelpText { get; }

        public bool Hidden { get; }

        public IReadOnlyList<OptionDocumentation> Options { get; } = Array.Empty<OptionDocumentation>();


        public CommandDocumentation(string name, string helpText = null, bool hidden = false, IEnumerable<OptionDocumentation> options = null)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Value must not be null or empty", nameof(name));

            Name = name;
            HelpText = helpText;
            Hidden = hidden;
            Options = options?.ToArray() ?? Array.Empty<OptionDocumentation>();
        }


        public static CommandDocumentation FromTypeDefinition(TypeDefinition definition, ILogger logger)
        {
            if (definition is null)
                throw new ArgumentNullException(nameof(definition));

            var verbAttribute = definition.CustomAttributes.Single(x => x.AttributeType.FullName == Constants.VerbAttributeFullName);

            var name = (string)verbAttribute.ConstructorArguments.Single().Value;
            var helpText = (string)verbAttribute.Properties.SingleOrDefault(x => x.Name == "HelpText").Argument.Value;
            var hidden = (verbAttribute.Properties.SingleOrDefault(x => x.Name == "Hidden").Argument.Value as bool?) ?? false;

            return new CommandDocumentation(
                name: name,
                helpText: helpText,
                hidden: hidden,
                options: LoadOptions(definition, logger));
        }


        private static IReadOnlyList<OptionDocumentation> LoadOptions(TypeDefinition definition, ILogger logger)
        {
            var commands = new List<OptionDocumentation>();

            foreach (var property in definition.Properties)
            {
                if (property.CustomAttributes.Any(x => x.AttributeType.FullName == Constants.OptionAttributeFullName))
                {
                    commands.Add(OptionDocumentation.FromPropertyDefinition(property, logger));
                }
            }

            return commands;
        }
    }
}
