using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Basic.Reference.Assemblies;
using Grynwald.Utilities.Collections;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Mono.Cecil;
using Xunit.Sdk;

namespace Grynwald.MdDocs.TestHelpers
{
    /// <summary>
    /// Test base class providing helper methods to dynamically compile C# code to an assembly using Roslyn.
    /// </summary>
    public abstract class DynamicCompilationTestBase
    {
        private const string s_DefaultAssemblyName = "DynamicTestAssembly";

        protected static readonly Lazy<IReadOnlyList<MetadataReference>> s_DefaultMetadataReferences = new Lazy<IReadOnlyList<MetadataReference>>(() =>
        {

#if NET472
            return ReferenceAssemblies.Net472.ToList();
#elif NET6_0_OR_GREATER
            return ReferenceAssemblies.Net60.ToList();
#else
            throw new NotImplementedException(); 
#endif
        });


        protected AssemblyDefinition Compile(string sourceCode, string assemblyName = s_DefaultAssemblyName) => Compile(sourceCode, out _, assemblyName);

        protected AssemblyDefinition Compile(string sourceCode, out XDocument xmlDocumentation, string assemblyName = s_DefaultAssemblyName)
        {
            var compilation = GetCompilation(sourceCode, assemblyName);

            using var assemblyStream = new MemoryStream();
            using var xmlDocumentationStream = new MemoryStream();

            var emitResult = compilation.Emit(peStream: assemblyStream, xmlDocumentationStream: xmlDocumentationStream);
            EnsureCompilationSucccess(emitResult);

            assemblyStream.Seek(0, SeekOrigin.Begin);
            xmlDocumentationStream.Seek(0, SeekOrigin.Begin);

            var assembly = AssemblyDefinition.ReadAssembly(assemblyStream);
            xmlDocumentation = XDocument.Load(xmlDocumentationStream);

            return assembly;
        }

        protected void CompileToFile(string sourceCode, string assemblyOutputPath, string xmlDocumenationOutputPath)
        {
            var compilation = GetCompilation(sourceCode, Path.GetFileNameWithoutExtension(assemblyOutputPath));

            using var assemblyStream = File.Open(assemblyOutputPath, FileMode.Create, FileAccess.Write);
            using var xmlDocumentationStream = File.Open(xmlDocumenationOutputPath, FileMode.Create, FileAccess.Write);

            var emitResult = compilation.Emit(peStream: assemblyStream, xmlDocumentationStream: xmlDocumentationStream);
            EnsureCompilationSucccess(emitResult);
        }

        protected Compilation GetCompilation(string sourceCode, string assemblyName = s_DefaultAssemblyName)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

            var compilation = CSharpCompilation.Create(
              assemblyName,
              new[] { syntaxTree },
              GetMetadataReferences(),
              new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            return compilation;
        }

        protected virtual IReadOnlyList<MetadataReference> GetMetadataReferences() => s_DefaultMetadataReferences.Value;


        private static void EnsureCompilationSucccess(EmitResult emitResult)
        {
            if (!emitResult.Success)
            {
                var errors = emitResult.Diagnostics
                 .Where(d => d.Severity >= DiagnosticSeverity.Error || d.IsWarningAsError)
                 .Select(d => d.GetMessage())
                .ToArray();

                throw new XunitException($"Failed to compile code to assembly:\r\n {errors.JoinToString("\r\n")}");
            }
        }
    }
}
