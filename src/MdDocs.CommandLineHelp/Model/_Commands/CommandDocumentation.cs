using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Represents a named command (in an application with sub-commands)
    /// </summary>
    public sealed class CommandDocumentation : CommandDocumentationBase
    {
        public string Name { get; }

        public string HelpText { get; }

        public bool Hidden { get; }


        public CommandDocumentation(
            MultiCommandApplicationDocumentation application,
            string name,
            string helpText = null,
            bool hidden = false,
            IEnumerable<OptionDocumentation> options = null,
            IEnumerable<ValueDocumentation> values = null) : base(application, options, values)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Value must not be null or empty", nameof(name));

            Name = name;
            HelpText = helpText;
            Hidden = hidden;
        }


        public static CommandDocumentation FromTypeDefinition(MultiCommandApplicationDocumentation application, TypeDefinition definition, ILogger logger)
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
    }
}
