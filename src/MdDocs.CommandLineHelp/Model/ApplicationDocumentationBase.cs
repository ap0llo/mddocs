using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.Common.Model;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    public abstract class ApplicationDocumentationBase
    {
        public string Name { get; }

        public string Version { get; }

        public IReadOnlyList<string> Usage { get; }


        internal ApplicationDocumentationBase(string name, string version = null, IEnumerable<string> usage = null)
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


        public static ApplicationDocumentationBase FromAssemblyFile(string filePath, ILogger logger)
        {
            using (var definition = AssemblyReader.ReadFile(filePath, logger))
            {
                var types = definition.MainModule.Types.Where(x => !x.IsAbstract);

                if(types.Any(x => x.HasAttribute(Constants.VerbAttributeFullName)))
                {
                    return MultiCommandApplicationDocumentation.FromAssemblyDefinition(definition, logger);
                }
                else
                {
                    return SingleCommandApplicationDocumentation.FromAssemblyDefinition(definition, logger);
                }
            }
        }


        

    }
}
