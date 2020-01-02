using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Grynwald.Utilities.Collections;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Mono.Cecil;
using Xunit.Sdk;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public abstract class DynamicCompilationTestBase
    {
        private static Lazy<MetadataReference[]> s_MetadataReferences = new Lazy<MetadataReference[]>(
            () => new MetadataReference[]
            {
                MetadataReference.CreateFromFile(Assembly.Load("netstandard").Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            });

        


        protected AssemblyDefinition Compile(string sourceCode)
        {
            var compilation = GetCompilation(sourceCode);

            using var stream = new MemoryStream();

            var emitResult = compilation.Emit(stream);
            if (!emitResult.Success)
            {
                var errors = emitResult.Diagnostics
                 .Where(d => d.Severity >= DiagnosticSeverity.Error || d.IsWarningAsError)
                 .Select(d => d.GetMessage())
                .ToArray();

                throw new XunitException($"Failed to compile code to assembly:\r\n {errors.JoinToString("\r\n")}");
            }

            stream.Seek(0, SeekOrigin.Begin);
            return AssemblyDefinition.ReadAssembly(stream);
        }

        protected Compilation GetCompilation(string sourceCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

            var assemblyName = "Compilation_" + Path.GetRandomFileName();

            var compilation = CSharpCompilation.Create(
              assemblyName,
              new[] { syntaxTree },
              s_MetadataReferences.Value,
              new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            return compilation;
        }
    }
}
