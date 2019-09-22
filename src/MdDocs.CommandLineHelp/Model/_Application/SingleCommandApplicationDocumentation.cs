using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Represents a application without any-subcommands.
    /// </summary>
    public class SingleCommandApplicationDocumentation : ApplicationDocumentation
    {
        public UnnamedCommandDocumentation Command { get; }


        public SingleCommandApplicationDocumentation(string name, UnnamedCommandDocumentation parameters, string version = null, IEnumerable<string> usage = null)
            : base(name, version, usage)
        {
            Command = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        private SingleCommandApplicationDocumentation(AssemblyDefinition definition, ILogger logger)
            : base(
                name: LoadApplicationName(definition ?? throw new ArgumentNullException(nameof(definition))),
                version: LoadApplicationVersion(definition ?? throw new ArgumentNullException(nameof(definition))),
                usage: LoadAssemblyUsage(definition))
        {
            Command = LoadCommand(definition, logger);
        }


        public static SingleCommandApplicationDocumentation FromAssemblyDefinition(AssemblyDefinition definition, ILogger logger) =>
            new SingleCommandApplicationDocumentation(definition, logger ?? throw new ArgumentNullException(nameof(logger)));


        private UnnamedCommandDocumentation LoadCommand(AssemblyDefinition definition, ILogger logger)
        {
            bool IsCommandLineParameter(PropertyDefinition property)
            {
                return property.HasAttribute(Constants.OptionAttributeFullName) || property.HasAttribute(Constants.ValueAttributeFullName);
            }

            // get all types with at least one property attributed as either [Option] or [Value]
            var optionTypes = definition.MainModule.Types
                .Where(x => !x.IsAbstract)
                .Where(type => type.Properties.Any(IsCommandLineParameter))
                .ToArray();

            TypeDefinition optionType;

            // no option classes found => return "empty" command (unnamed command without options or values)
            if (optionTypes.Length == 0)
            {
                logger.LogWarning("No option classes found.");
                return new UnnamedCommandDocumentation(this);
            }
            // use the first option class if multiple candidates were found but log a warning
            else if (optionTypes.Length > 1)
            {
                optionType = optionTypes[0];
                var ignoredTypeNames = optionTypes.Skip(1).Select(x => x.FullName);
                logger.LogWarning(
                    $"Multiple option classes found. Generating documentation for type {optionType.FullName}. " +
                    $"Ignored types: {String.Join(", ", ignoredTypeNames)}");
            }
            // single option class found
            else
            {
                optionType = optionTypes[0];
            }

            return UnnamedCommandDocumentation.FromTypeDefinition(this, optionType, logger);
        }
    }
}
