using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    public abstract class ApplicationDocumentationBase
    {
        public string Name { get; }

        public string Version { get; }

        public IReadOnlyList<string> Usage { get; }


        // protected internal: Prevent implementations outside the assembly (abstract classes cannot be sealed)
        protected internal ApplicationDocumentationBase(string name, string version = null, IEnumerable<string> usage = null)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value must not be null or whitespace", nameof(name));
            }

            Name = name;
            Version = version;
            Usage = usage?.ToArray() ?? Array.Empty<string>();
        }


        protected static string LoadApplicationName(AssemblyDefinition definition)
        {
            var name = definition
                .GetAttributeOrDefault(Constants.AssemblyTitleAttributeFullName)
                ?.ConstructorArguments?.Single().Value as string;

            if (String.IsNullOrEmpty(name))
            {
                name = definition.Name.Name;
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
                version = definition.Name.Version.ToString();
            }

            return version;
        }

        protected static IReadOnlyList<string> LoadAssemblyUsage(AssemblyDefinition definition)
        {
            var assemblyUsageAttribute = definition.GetAttributeOrDefault(Constants.AssemblyUsageAttributeFullName);
            if (assemblyUsageAttribute != null)
            {
                return assemblyUsageAttribute.ConstructorArguments.Select(x => x.Value).Cast<string>().ToArray();
            }
            else
            {
                return Array.Empty<string>();
            }
        }
    }
}
