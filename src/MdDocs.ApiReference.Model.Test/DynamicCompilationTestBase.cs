using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Grynwald.Utilities.Collections;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Mono.Cecil;
using Xunit.Sdk;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public abstract class DynamicCompilationTestBase
    {
        private static readonly Lazy<IReadOnlyList<MetadataReference>> s_MetadataReferences = new Lazy<IReadOnlyList<MetadataReference>>(() =>
        {
            var paths = new HashSet<string>()
            {
                Assembly.Load("netstandard").Location,
                Assembly.Load("System.Runtime").Location,
                typeof(object).Assembly.Location,
                typeof(DirectoryInfo).Assembly.Location,
                typeof(ConsoleColor).Assembly.Location,
            };

            return paths.Select(p => MetadataReference.CreateFromFile(p)).ToArray();
        });


        protected AssemblyDefinition Compile(string sourceCode) => Compile(sourceCode, out _);

        protected AssemblyDefinition Compile(string sourceCode, out XDocument xmlDocumentation)
        {
            var compilation = GetCompilation(sourceCode);

            using var assemblyStream = new MemoryStream();
            using var xmlDocumentationStream = new MemoryStream();

            var emitResult = compilation.Emit(peStream: assemblyStream, xmlDocumentationStream: xmlDocumentationStream);
            if (!emitResult.Success)
            {
                var errors = emitResult.Diagnostics
                 .Where(d => d.Severity >= DiagnosticSeverity.Error || d.IsWarningAsError)
                 .Select(d => d.GetMessage())
                .ToArray();

                throw new XunitException($"Failed to compile code to assembly:\r\n {errors.JoinToString("\r\n")}");
            }

            assemblyStream.Seek(0, SeekOrigin.Begin);
            xmlDocumentationStream.Seek(0, SeekOrigin.Begin);

            var assembly = AssemblyDefinition.ReadAssembly(assemblyStream);
            xmlDocumentation = XDocument.Load(xmlDocumentationStream);

            return assembly;
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
