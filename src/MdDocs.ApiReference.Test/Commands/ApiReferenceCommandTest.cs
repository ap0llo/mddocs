﻿using System;
using System.IO;
using Grynwald.MdDocs.ApiReference.Commands;
using Grynwald.MdDocs.ApiReference.Configuration;
using Grynwald.MdDocs.TestHelpers;
using Grynwald.Utilities.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Commands
{
    /// <summary>
    /// Tests for <see cref="ApiReferenceCommand"/>
    /// </summary>
    public class ApiReferenceCommandTest : DynamicCompilationTestBase
    {
        private readonly ILogger m_Logger = NullLogger.Instance;


        [Fact]
        public void Execute_returns_false_if_AssemblyPaths_is_empty()
        {
            // ARRANGE
            var configuration = new ApiReferenceConfiguration()
            {
                AssemblyPaths = Array.Empty<string>(),
                OutputPath = "./some-output-path"
            };

            var sut = new ApiReferenceCommand(m_Logger, configuration);

            // ACT 
            var success = sut.Execute();

            // ASSERT
            Assert.False(success);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData("does-not-exists.dll")]
        public void Execute_returns_false_if_AssemblyPaths_is_invalid(string assemblyPath)
        {
            // ARRANGE
            var configuration = new ApiReferenceConfiguration()
            {
                AssemblyPaths = new[] { assemblyPath },
                OutputPath = "./some-output-path"
            };

            var sut = new ApiReferenceCommand(m_Logger, configuration);

            // ACT 
            var success = sut.Execute();

            // ASSERT
            Assert.False(success);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("\t")]
        public void Execute_returns_false_if_OutputPath_is_invalid(string outputPath)
        {
            // ARRANGE
            using var temporaryDirectory = new TemporaryDirectory();
            var assemblyPath = Path.Combine(temporaryDirectory, "myAssembly.dll");
            File.WriteAllText(assemblyPath, "");

            var configuration = new ApiReferenceConfiguration()
            {
                AssemblyPaths = new[] { assemblyPath },
                OutputPath = outputPath
            };

            var sut = new ApiReferenceCommand(m_Logger, configuration);

            // ACT 
            var success = sut.Execute();

            // ASSERT
            Assert.False(success);
        }

        [Fact]
        public void Execute_generates_apireference_output()
        {
            // ARRANGE
            using var temporaryDirectory = new TemporaryDirectory();

            var assemblyPath = Path.Combine(temporaryDirectory, $"myAssembly.dll");
            var xmlDocumentationPath = Path.ChangeExtension(assemblyPath, ".xml");
            var outputPath = Path.Combine(temporaryDirectory, "output");

            CompileToFile(@"
                namespace MyNamespace
                {
                    public class Class1
                    {                   
                        public string Option1 { get; set; }
                    }
                }
            ", assemblyPath, xmlDocumentationPath);

            var configuration = new ApiReferenceConfiguration()
            {
                AssemblyPaths = new[] { assemblyPath },
                OutputPath = outputPath
            };

            var sut = new ApiReferenceCommand(m_Logger, configuration);

            // ACT 
            var success = sut.Execute();

            // ASSERT
            Assert.True(success);
            Assert.True(Directory.Exists(outputPath));
            Assert.True(File.Exists(Path.Combine(outputPath, @"MyNamespace\index.md")));
            Assert.True(File.Exists(Path.Combine(outputPath, @"MyNamespace\Class1\index.md")));
            Assert.True(File.Exists(Path.Combine(outputPath, @"MyNamespace\Class1\constructors\index.md")));
            Assert.True(File.Exists(Path.Combine(outputPath, @"MyNamespace\Class1\properties\Option1.md")));
        }
    }
}
