#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.Common.Model;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    /// <summary>
    /// Base class for model describing an application.
    /// </summary>
    public abstract class ApplicationDocumentation : IDisposable
    {
        public string Name { get; }

        public string Version { get; }

        public IReadOnlyList<string> Usage { get; }


        internal ApplicationDocumentation(string name, string version = null, IEnumerable<string> usage = null)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value must not be null or whitespace", nameof(name));

            Name = name;
            Version = version;
            Usage = usage?.ToArray() ?? Array.Empty<string>();
        }


        public static ApplicationDocumentation FromAssemblyFile(string filePath, ILogger logger)
        {
            using (var definition = AssemblyReader.ReadFile(filePath, logger))
            {
                var types = definition.MainModule.Types.Where(x => !x.IsAbstract);

                if (types.Any(x => x.HasAttribute(Constants.VerbAttributeFullName)))
                {
                    logger.LogInformation($"Found a class attributed with '{Constants.VerbAttributeFullName}'. Assuming application has sub-commands");
                    return MultiCommandApplicationDocumentation.FromAssemblyDefinition(definition, logger);
                }
                else
                {
                    logger.LogInformation($"Found *no* class attributed with '{Constants.VerbAttributeFullName}'. Assuming application without sub-commands");
                    return SingleCommandApplicationDocumentation.FromAssemblyDefinition(definition, logger);
                }
            }
        }


        protected static string LoadApplicationName(AssemblyDefinition definition)
        {
            var name = definition
                .GetAttributeOrDefault(Constants.AssemblyTitleAttributeFullName)
                ?.ConstructorArguments?.Single().Value as string;

            if (String.IsNullOrEmpty(name))
            {
                // no AssemblyTitle specified => return assembly name
                return definition.Name.Name;
            }

            return name;
        }

        protected static string LoadApplicationVersion(AssemblyDefinition definition)
        {
            var version = definition
              .GetAttributeOrDefault(Constants.AssemblyInformationalVersionAttribute)
              ?.ConstructorArguments.Single().Value as string;

            if (String.IsNullOrEmpty(version))
            {
                // no AssemblyInformationalVersion found => return assembly version
                version = definition.Name.Version.ToString();
            }

            return version;
        }

        protected static IReadOnlyList<string> LoadAssemblyUsage(AssemblyDefinition definition)
        {
            var assemblyUsageAttribute = definition.GetAttributeOrDefault(Constants.AssemblyUsageAttributeFullName);

            return assemblyUsageAttribute == null
                ? Array.Empty<string>()
                : assemblyUsageAttribute.ConstructorArguments.Select(x => x.Value).Cast<string>().ToArray();
        }

        public void Dispose()
        {
            //nop
        }
    }
}
