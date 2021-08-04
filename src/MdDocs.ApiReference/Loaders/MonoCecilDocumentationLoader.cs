using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.Common;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Loaders
{
    public class MonoCecilDocumentationLoader : IDocumentationLoader
    {
        private readonly ILogger m_Logger;


        public MonoCecilDocumentationLoader(ILogger logger)
        {
            m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public _AssemblySetDocumentation Load(IEnumerable<AssemblyDefinition> assemblyDefinitions)
        {
            m_Logger.LogDebug("Loading assembly set");

            var builder = new ApiReferenceBuilder();

            foreach (var assemblyDefinition in assemblyDefinitions)
            {
                var assemblyName = assemblyDefinition.Name.Name;
                m_Logger.LogInformation($"Loading assembly '{assemblyName}'");
                _ = builder.AddAssembly(assemblyName, assemblyDefinition.GetInformationalVersionOrVersion());

                foreach (var typeDefinition in assemblyDefinition.MainModule.Types.Where(t => t.IsPublic))
                {
                    LoadTypeRecursively(builder, typeDefinition);
                }
            }

            return builder.Build();
        }


        private void LoadTypeRecursively(ApiReferenceBuilder builder, TypeDefinition typeDefinition)
        {
            var assemblyName = typeDefinition.Module.Assembly.Name.Name;
            var typeId = typeDefinition.ToTypeId();
            m_Logger.LogDebug($"Loading type '{typeId}' from assembly '{assemblyName}'");
            _ = builder.AddType(assemblyName, typeId);

            if (typeDefinition.HasNestedTypes)
            {
                foreach (var nestedType in typeDefinition.NestedTypes.Where(x => x.IsNestedPublic))
                {
                    LoadTypeRecursively(builder, nestedType);
                }
            }
        }
    }
}
