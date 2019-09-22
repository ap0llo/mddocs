using System.IO;
using Microsoft.Extensions.Logging;
using Mono.Cecil;

namespace Grynwald.MdDocs.Common.Model
{
    public static class AssemblyReader
    {
        public static AssemblyDefinition ReadFile(string filePath, ILogger logger)
        {
            var dir = Path.GetDirectoryName(filePath);

            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(dir);

            // load assembly
            logger.LogInformation($"Loading assembly from '{filePath}'");
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(filePath, new ReaderParameters() { AssemblyResolver = assemblyResolver });

            return assemblyDefinition;
        }
    }
}
