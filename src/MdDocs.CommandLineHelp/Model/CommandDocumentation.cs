using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    public sealed class CommandDocumentation
    {
        public ApplicationDocumentation Application { get; }

        public string Name { get; }

        public string HelpText { get; }

        public bool Hidden { get; }

        public IReadOnlyList<OptionDocumentation> Options { get; } = Array.Empty<OptionDocumentation>();

        public IReadOnlyList<ValueDocumentation> Values { get; } = Array.Empty<ValueDocumentation>();

        public IReadOnlyList<ParameterDocumentation> Parameters { get; } = Array.Empty<ParameterDocumentation>();


        public CommandDocumentation(
            ApplicationDocumentation application,
            string name,
            string helpText = null,
            bool hidden = false,
            IEnumerable<OptionDocumentation> options = null,
            IEnumerable<ValueDocumentation> values = null)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Value must not be null or empty", nameof(name));

            Application = application ?? throw new ArgumentNullException(nameof(application));
            Name = name;
            HelpText = helpText;
            Hidden = hidden;
            Options = options?.Where(x => !x.Hidden)?.OrderBy(x => x.Name ?? x.ShortName?.ToString())?.ToArray() ?? Array.Empty<OptionDocumentation>();
            Values = values?.Where(x => !x.Hidden)?.OrderBy(x => x.Index)?.ToArray() ?? Array.Empty<ValueDocumentation>();
            Parameters = Values.Cast<ParameterDocumentation>().Concat(Options).ToArray();
        }


        public static CommandDocumentation FromTypeDefinition(ApplicationDocumentation application, TypeDefinition definition, ILogger logger)
        {
            if (application is null)
                throw new ArgumentNullException(nameof(application));

            if (definition is null)
                throw new ArgumentNullException(nameof(definition));

            var verbAttribute = definition.GetAttribute(Constants.VerbAttributeFullName);

            var name = (string)verbAttribute.ConstructorArguments.Single().Value;

            var helpText = verbAttribute.GetPropertyValueOrDefault<string>("HelpText");
            var hidden = verbAttribute.GetPropertyValueOrDefault<bool>("Hidden");

            return new CommandDocumentation(
                application: application,
                name: name,
                helpText: helpText,
                hidden: hidden,
                options: LoadOptions(definition, logger),
                values: LoadValues(definition, logger));
        }


        private static IReadOnlyList<OptionDocumentation> LoadOptions(TypeDefinition definition, ILogger logger)
        {
            return definition.Properties
                .WithAttribute(Constants.OptionAttributeFullName)
                .Select(property => OptionDocumentation.FromPropertyDefinition(property, logger))
                .Where(option => !option.Hidden)
                .ToArray();
        }

        private static IReadOnlyList<ValueDocumentation> LoadValues(TypeDefinition definition, ILogger logger)
        {
            return definition.Properties
                .WithAttribute(Constants.ValueAttributeFullName)
                .Select(property => ValueDocumentation.FromPropertyDefinition(property, logger))
                .Where(value => !value.Hidden)
                .ToArray();
        }
    }
}
