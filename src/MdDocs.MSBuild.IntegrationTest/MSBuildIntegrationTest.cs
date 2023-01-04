using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grynwald.MdDocs.TestHelpers;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;
using Xunit.Abstractions;

namespace Grynwald.MdDocs.MSBuild.IntegrationTest
{
    public class MSBuildIntegrationTest : IClassFixture<PackagesFixture>
    {
        private readonly ITestOutputHelper m_OutputHelper;
        private readonly PackageInfo m_MdDocsPackage;


        public MSBuildIntegrationTest(ITestOutputHelper outputHelper, PackagesFixture packagesFixture)
        {
            m_OutputHelper = outputHelper;
            m_MdDocsPackage = packagesFixture.GetPackage("Grynwald.MdDocs.MSBuild");
        }


        public static IEnumerable<object[]> MSBuildRuntimesData() => MSBuildRuntimes.All.Select(x => new object[] { x });


        [Theory]
        [MemberData(nameof(MSBuildRuntimesData))]
        public void ApiReference_command_is_started_during_build(MSBuildRuntimeInfo msbuildRuntime)
        {
            // ARRANGE
            using var project = new MSBuildTestProject(m_OutputHelper);

            var mddocsCliAssemblyPath = project.ResolveFileFromPackage(m_MdDocsPackage.Identity, "tools/net6.0/mddocs.dll");

            // Paths
            var configuredApiReferenceOutputDirectory = "api/";
            var absoluteApiReferenceOutputDirectory = project.ProjectDirectory.PathCombine(configuredApiReferenceOutputDirectory).GetFullPath().TrimEndingDirectorySeparator();

            // Expected arguments
            var expectedArguments = new ProcessArgumentsBuilder()
                .Append("dotnet").AppendQuoted(mddocsCliAssemblyPath)
                .Append("apireference")
                .Append("--assemblies").AppendQuoted(project.TargetPath)
                .Append("--outdir").AppendQuoted(absoluteApiReferenceOutputDirectory);

            // Set up project
            project
                .InstallNuGetPackage(m_MdDocsPackage)
                .ConfigureMdDocs(x =>
                {
                    x.GenerateApiReferenceDocumentationOnBuild = true;
                    x.ApiReferenceDocumentationOutputPath = configuredApiReferenceOutputDirectory;
                });

            // ACT
            var result = project.Build(msbuildRuntime);

            // ASSERT
            Assert.Equal(0, result.ExitCode);
            AssertCommandLineArguments(expectedArguments, result, "GenerateApiReferenceDocumentation");
        }

        [Theory]
        [InlineData(new string[0], null, null)]
        [InlineData(new string[] { "mddocs.settings.json" }, null, "mddocs.settings.json")]
        [InlineData(new string[] { "mddocs.settings.json", "config/mddocs/settings.json" }, null, "mddocs.settings.json")]
        [InlineData(new string[] { "mddocs.settings.json", "config/mddocs/settings.json" }, "config/mddocs/settings.json", "config/mddocs/settings.json")]
        public void ApiReference_command_is_started_with_the_expected_configuration_file_parameter(string[] configurationFilesOnDisk, string? configuredConfigurationFile, string expectedConfigurationFile)
        {
            // ARRANGE
            using var project = new MSBuildTestProject(m_OutputHelper);

            var mddocsCliAssemblyPath = project.ResolveFileFromPackage(m_MdDocsPackage.Identity, "tools/net6.0/mddocs.dll");

            // Expected arguments
            var expectedArguments = new ProcessArgumentsBuilder()
                .Append("dotnet").AppendQuoted(mddocsCliAssemblyPath)
                .Append("apireference")
                .Append("--assemblies").AppendQuoted(project.TargetPath);

            if (expectedConfigurationFile is not null)
            {
                expectedArguments
                    .Append("--configurationFilePath")
                    .AppendQuoted(project.ProjectDirectory.PathCombine(expectedConfigurationFile).GetFullPath());
            }

            // Set up project
            project
                .InstallNuGetPackage(m_MdDocsPackage)
                .ConfigureMdDocs(x =>
                {
                    x.GenerateApiReferenceDocumentationOnBuild = true;
                    x.ConfigurationFilePath = configuredConfigurationFile;
                });

            foreach (var configurationFile in configurationFilesOnDisk)
            {
                project.AddFile(configurationFile, "{ }");
            }

            // ACT
            var result = project.Build(MSBuildRuntimes.Default);

            // ASSERT
            AssertCommandLineArguments(expectedArguments, result, "GenerateApiReferenceDocumentation");
        }

        [Fact]
        public void ApiReference_Expected_files_are_generated_during_build()
        {
            // ARRANGE
            using var project = new MSBuildTestProject(m_OutputHelper);

            // Paths
            var configuredApiReferenceOutputDirectory = "api/";
            var absoluteApiReferenceOutputDirectory = project.ProjectDirectory.PathCombine(configuredApiReferenceOutputDirectory).GetFullPath().TrimEndingDirectorySeparator();

            // Set up project
            project
                .InstallNuGetPackage(m_MdDocsPackage)
                .AddFile("Class1.cs",
                    """
                    namespace MyNamespace
                    {
                        public class Class1
                        {
                        }
                    }
                    """)
                .ConfigureMdDocs(x =>
                {
                    x.GenerateApiReferenceDocumentationOnBuild = true;
                    x.ApiReferenceDocumentationOutputPath = configuredApiReferenceOutputDirectory;
                });

            // ACT
            var result = project.Build(MSBuildRuntimes.Default);

            // ASSERT
            Assert.Equal(0, result.ExitCode);

            var files = Directory
                    .GetFiles(absoluteApiReferenceOutputDirectory, "*", SearchOption.AllDirectories)
                    .Select(x => Path.GetRelativePath(absoluteApiReferenceOutputDirectory, x))
                    .Select(x => x.Replace("\\", "/"))
                    .Order(StringComparer.Ordinal);

            Assert.Collection(
                files,
                x => Assert.Equal("MyNamespace/Class1/constructors/index.md", x),
                x => Assert.Equal("MyNamespace/Class1/index.md", x),
                x => Assert.Equal("MyNamespace/index.md", x)
            );
        }

        [Fact]
        public void ApiReference_Warnings_written_to_the_console_are_emitted_as_MSBuild_warnings()
        {
            // ARRANGE
            using var project = new MSBuildTestProject(m_OutputHelper);

            // Set up project
            project
                .InstallNuGetPackage(m_MdDocsPackage)
                .ConfigureMdDocs(x =>
                {
                    x.GenerateApiReferenceDocumentationOnBuild = true;
                    x.ApiReferenceDocumentationOutputPath = "api/";
                });

            // ACT
            var result = project.Build(MSBuildRuntimes.Default);

            // ASSERT
            var target = Assert.Single(result.BinaryLog.GetTargets("GenerateApiReferenceDocumentation"));
            var execTask = Assert.Single(target.GetTasks("Exec"));

            var warnings = execTask.FindChildrenRecursive<Warning>();
            Assert.Contains(warnings, warning => warning.Text.Contains("WARNING - No XML documentation file for assembly found"));

        }

        [Fact]
        public void ApiReference_Errors_written_to_the_console_are_emitted_as_MSBuild_errors()
        {
            // ARRANGE
            using var project = new MSBuildTestProject(m_OutputHelper);

            // Set up project
            project
                .InstallNuGetPackage(m_MdDocsPackage)
                .ConfigureMdDocs(x => x.GenerateApiReferenceDocumentationOnBuild = true);

            // ACT
            var result = project.Build(MSBuildRuntimes.Default);

            // ASSERT
            var target = Assert.Single(result.BinaryLog.GetTargets("GenerateApiReferenceDocumentation"));
            var execTask = Assert.Single(target.GetTasks("Exec"));

            var errors = execTask.FindChildrenRecursive<Error>();
            Assert.Contains(errors, err => err.Text.Contains("ERROR - Invalid output directory"));
        }

        [Theory]
        [MemberData(nameof(MSBuildRuntimesData))]
        public void CommandLineHelp_command_is_started_during_build(MSBuildRuntimeInfo msbuildRuntime)
        {
            // ARRANGE
            using var project = new MSBuildTestProject(m_OutputHelper);

            var mddocsCliAssemblyPath = project.ResolveFileFromPackage(m_MdDocsPackage.Identity, "tools/net6.0/mddocs.dll");

            // Paths
            var configuredOutputDirectory = "commandlinehelp/";
            var absoluteOutputDirectory = project.ProjectDirectory.PathCombine(configuredOutputDirectory).GetFullPath().TrimEndingDirectorySeparator();

            // Expected arguments
            var expectedArguments = new ProcessArgumentsBuilder()
                .Append("dotnet").AppendQuoted(mddocsCliAssemblyPath)
                .Append("commandlinehelp")
                .Append("--assembly").AppendQuoted(project.TargetPath)
                .Append("--outdir").AppendQuoted(absoluteOutputDirectory);

            // Set up project
            project
                .InstallNuGetPackage(m_MdDocsPackage)
                .ConfigureMdDocs(x =>
                {
                    x.GenerateCommandLineDocumentationOnBuild = true;
                    x.CommandLineDocumentationOutputPath = configuredOutputDirectory;
                });

            // ACT
            var result = project.Build(msbuildRuntime);

            // ASSERT
            Assert.Equal(0, result.ExitCode);
            AssertCommandLineArguments(expectedArguments, result, "GenerateCommandLineDocumentation");
        }

        [Theory]
        [InlineData(new string[0], null, null)]
        [InlineData(new string[] { "mddocs.settings.json" }, null, "mddocs.settings.json")]
        [InlineData(new string[] { "mddocs.settings.json", "config/mddocs/settings.json" }, null, "mddocs.settings.json")]
        [InlineData(new string[] { "mddocs.settings.json", "config/mddocs/settings.json" }, "config/mddocs/settings.json", "config/mddocs/settings.json")]
        public void CommandLineHelp_command_is_started_with_the_expected_configuration_file_parameter(string[] configurationFilesOnDisk, string? configuredConfigurationFile, string expectedConfigurationFile)
        {
            // ARRANGE
            using var project = new MSBuildTestProject(m_OutputHelper);

            var mddocsCliAssemblyPath = project.ResolveFileFromPackage(m_MdDocsPackage.Identity, "tools/net6.0/mddocs.dll");

            // Expected arguments
            var expectedArguments = new ProcessArgumentsBuilder()
                .Append("dotnet").AppendQuoted(mddocsCliAssemblyPath)
                .Append("commandlinehelp")
                .Append("--assembly").AppendQuoted(project.TargetPath);

            if (expectedConfigurationFile is not null)
            {
                expectedArguments
                    .Append("--configurationFilePath")
                    .AppendQuoted(project.ProjectDirectory.PathCombine(expectedConfigurationFile).GetFullPath());
            }

            // Set up project
            project
                .InstallNuGetPackage(m_MdDocsPackage)
                .ConfigureMdDocs(x =>
                {
                    x.GenerateCommandLineDocumentationOnBuild = true;
                    x.ConfigurationFilePath = configuredConfigurationFile;
                });

            foreach (var configurationFile in configurationFilesOnDisk)
            {
                project.AddFile(configurationFile, "{ }");
            }

            // ACT
            var result = project.Build(MSBuildRuntimes.Default);

            // ASSERT
            AssertCommandLineArguments(expectedArguments, result, "GenerateCommandLineDocumentation");
        }

        [Fact]
        public void CommandLineHelp_Expected_files_are_generated_during_build()
        {
            // ARRANGE
            using var project = new MSBuildTestProject(m_OutputHelper);

            // Paths
            var configuredOutputDirectory = "commandlinehelp/";
            var absoluteOutputDirectory = project.ProjectDirectory.PathCombine(configuredOutputDirectory).GetFullPath().TrimEndingDirectorySeparator();

            // Set up project
            project
                .InstallNuGetPackage(m_MdDocsPackage)
                .AddFile("Class1.cs",
                    """
                    namespace MyNamespace
                    {
                        public class Class1
                        {
                        }
                    }
                    """)
                .ConfigureMdDocs(x =>
                {
                    x.GenerateCommandLineDocumentationOnBuild = true;
                    x.CommandLineDocumentationOutputPath = configuredOutputDirectory;
                });

            // ACT
            var result = project.Build(MSBuildRuntimes.Default);

            // ASSERT
            Assert.Equal(0, result.ExitCode);

            var files = Directory
                    .GetFiles(absoluteOutputDirectory, "*", SearchOption.AllDirectories)
                    .Select(x => Path.GetRelativePath(absoluteOutputDirectory, x))
                    .Select(x => x.Replace("\\", "/"))
                    .Order(StringComparer.Ordinal);

            Assert.Collection(
                files,
                x => Assert.Equal("index.md", x)
            );
        }

        [Fact]
        public void CommandLineHelp_Warnings_written_to_the_console_are_emitted_as_MSBuild_warnings()
        {
            // ARRANGE
            using var project = new MSBuildTestProject(m_OutputHelper);

            // Set up project
            project
                .InstallNuGetPackage(m_MdDocsPackage)
                .ConfigureMdDocs(x =>
                {
                    x.GenerateCommandLineDocumentationOnBuild = true;
                    x.CommandLineDocumentationOutputPath = "commandlinehelp/";
                });

            // ACT
            var result = project.Build(MSBuildRuntimes.Default);

            // ASSERT
            var target = Assert.Single(result.BinaryLog.GetTargets("GenerateCommandLineDocumentation"));
            var execTask = Assert.Single(target.GetTasks("Exec"));

            var warnings = execTask.FindChildrenRecursive<Warning>();
            Assert.Contains(warnings, warning => warning.Text.Contains("WARNING - No option classes found"));

        }

        [Fact]
        public void CommandLineHelp_Errors_written_to_the_console_are_emitted_as_MSBuild_errors()
        {
            // ARRANGE
            using var project = new MSBuildTestProject(m_OutputHelper);

            // Set up project
            project
                .InstallNuGetPackage(m_MdDocsPackage)
                .ConfigureMdDocs(x => x.GenerateCommandLineDocumentationOnBuild = true);

            // ACT
            var result = project.Build(MSBuildRuntimes.Default);

            // ASSERT
            var target = Assert.Single(result.BinaryLog.GetTargets("GenerateCommandLineDocumentation"));
            var execTask = Assert.Single(target.GetTasks("Exec"));

            var errors = execTask.FindChildrenRecursive<Error>();
            Assert.Contains(errors, err => err.Text.Contains("ERROR - Invalid output directory"));
        }


        private void AssertCommandLineArguments(ProcessArgumentsBuilder expected, MSBuildExecutionResult msbuildResult, string targetName)
        {
            var target = Assert.Single(msbuildResult.BinaryLog.GetTargets(targetName));
            var execTask = Assert.Single(target.GetTasks("Exec"));
            Assert.Equal(expected.ToString(), execTask.CommandLineArguments);
        }
    }
}
