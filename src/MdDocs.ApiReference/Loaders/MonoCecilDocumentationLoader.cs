using System;
using System.Collections.Generic;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.Common;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Loaders
{
    public class MonoCecilDocumentationLoader : IDocumentationLoader
    {
        public _AssemblySetDocumentation Load(IEnumerable<AssemblyDefinition> assemblyDefinitions)
        {
            var duplicateAssemblyNames = assemblyDefinitions.DuplicatesBy(assembly => assembly.Name.Name, StringComparer.OrdinalIgnoreCase);
            if (duplicateAssemblyNames.Any())
            {
                throw new InvalidAssemblySetException($"Assembly set cannot contain multiple assemblies named {duplicateAssemblyNames.First()}");
            }

            var builder = new AssemblySetDocumentationBuilder();

            foreach (var assemblyDefinition in assemblyDefinitions)
            {
                _ = builder.GetOrAddAssembly(assemblyDefinition.Name.Name, assemblyDefinition.GetInformationalVersionOrVersion());

                foreach (var typeDefinition in assemblyDefinition.MainModule.Types.Where(t => t.IsPublic))
                {
                    LoadTypeRecursively(builder, typeDefinition, declaringType: null);
                }
            }

            return builder.Build();
        }


        private void LoadTypeRecursively(AssemblySetDocumentationBuilder builder, TypeDefinition typeDefinition, TypeDocumentation? declaringType)
        {
            //TODO 2021-08-04: Load types

            var typeId = typeDefinition.ToTypeId();
            //if (m_Types.ContainsKey(typeId))
            //{
            //    throw new InvalidAssemblySetException($"Type '{typeDefinition.FullName}' exists in multiple assemblies.");
            //}

            _ = builder.GetOrAddNamespace(typeId.Namespace.Name);

            //var typeDocumentation = new TypeDocumentation(assemblyDocumentation, namespaceDocumentation, typeDefinition, xmlDocsProvider, m_Logger, declaringType);
            //declaringType?.AddNestedType(typeDocumentation);

            //m_Types.Add(typeId, typeDocumentation);

            //namespaceDocumentation.AddType(typeDocumentation);
            //assemblyDocumentation.AddType(typeDocumentation);

            //if (typeDefinition.HasNestedTypes)
            //{
            //    foreach (var nestedType in typeDefinition.NestedTypes.Where(x => x.IsNestedPublic))
            //    {
            //        LoadTypeRecursively(namespaces, nestedType, typeDocumentation);
            //    }
            //}
        }
    }
}
