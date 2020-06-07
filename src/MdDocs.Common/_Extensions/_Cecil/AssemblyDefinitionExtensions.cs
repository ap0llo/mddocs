using System;
using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.Common
{
    public static class AssemblyDefinitionExtensions
    {
        /// <summary>
        /// Gets the assembly's informational version or the assembly version, if no informational version is defined.
        /// </summary>
        public static string? GetInformationalVersionOrVersion(this AssemblyDefinition definition)
        {
            var version = definition
              .GetAttributeOrDefault(SystemTypeNames.AssemblyInformationalVersionAttribute)
              ?.ConstructorArguments.Single().Value as string;

            if (String.IsNullOrEmpty(version))
            {
                // no AssemblyInformationalVersion found => return assembly version
                version = definition.Name.Version.ToString();
            }

            return version;
        }
    }
}
