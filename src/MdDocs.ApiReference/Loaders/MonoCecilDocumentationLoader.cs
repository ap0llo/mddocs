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

            var type = builder.AddType(assemblyName, typeId);

            type.Kind = typeDefinition.Kind();

            foreach (var fieldDefinition in typeDefinition.Fields.Where(field => field.IsPublic && !field.Attributes.HasFlag(FieldAttributes.SpecialName)))
            {
                m_Logger.LogDebug($"Loading field '{fieldDefinition.Name}'");
                var field = new _FieldDocumentation(type, fieldDefinition.Name, fieldDefinition.FieldType.ToTypeId());
                type.Add(field);
            }

            foreach (var eventDefinition in typeDefinition.Events.Where(ev => (ev.AddMethod?.IsPublic == true || ev.RemoveMethod?.IsPublic == true)))
            {
                m_Logger.LogDebug($"Loading event '{eventDefinition.Name}'");
                var @event = new _EventDocumentation(type, eventDefinition.Name, eventDefinition.EventType.ToTypeId());
                type.Add(@event);
            }

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
