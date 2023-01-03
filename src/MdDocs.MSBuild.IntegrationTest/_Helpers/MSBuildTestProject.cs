using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Grynwald.Utilities.IO;
using NuGet.Packaging;
using Xunit.Abstractions;

namespace Grynwald.MdDocs.MSBuild.IntegrationTest
{
    /// <summary>
    /// Helper class to dynamically create and build C# MSBuild projects
    /// </summary>
    internal class MSBuildTestProject : IDisposable
    {
        /// <summary>
        /// Encapsualtes MdDocs-specific project settings
        /// </summary>
        public class MdDocsProjectSettings
        {
            public bool? GenerateApiReferenceDocumentationOnBuild { get; set; }

            public bool? GenerateCommandLineDocumentationOnBuild { get; set; }

            public string? ApiReferenceDocumentationOutputPath { get; set; }

            public string? CommandLineDocumentationOutputPath { get; set; }

            public string? ConfigurationFilePath { get; set; }
        }

        private const string s_ProjectFileRelativePath = "TestProject.csproj";
        private const string s_TargetFramework = "netstandard2.0";

        private readonly ITestOutputHelper m_TestOutputHelper;
        private readonly TemporaryDirectory m_WorkingDirectory = new();
        private readonly List<string> m_PropertyImports = new();
        private readonly List<string> m_TargetImports = new();
        private readonly Dictionary<string, string> m_Files = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<(string id, string version, string filePath)> m_NuGetPackages = new();
        private readonly MdDocsProjectSettings m_MdDocsSettings = new();


        /// <summary>
        /// Gets the absolute path of the C# project file.
        /// </summary>
        public string ProjectPath => ProjectDirectory.PathCombine(s_ProjectFileRelativePath);

        /// <summary>
        /// Gets the absolute file of the project directory.
        /// </summary>
        public string ProjectDirectory => m_WorkingDirectory.PathCombine("src");

        /// <summary>
        /// Gets the path of the test-specific NuGet package source
        /// </summary>
        public string LocalNuGetPackageSourcePath => m_WorkingDirectory.PathCombine("packageSource");

        /// <summary>
        /// Gets the path of the test-specific NuGet cache ("global packages folder")
        /// </summary>
        public string NuGetCachePath => m_WorkingDirectory.PathCombine("nuget-cache");

        /// <summary>
        /// Gets the root path of the build output for the project
        /// </summary>
        public string RootOutputDirectory => m_WorkingDirectory.PathCombine("bin");

        /// <summary>
        /// Gets the absolute path of the project's output assembly.
        /// </summary>
        public string TargetPath => RootOutputDirectory.PathCombine("Debug", s_TargetFramework, "TestProject.dll");


        /// <summary>
        /// Initializes a new instance of <see cref="MSBuildTestProject"/>
        /// </summary>
        /// <param name="testOutputHelper">The Xunit <see cref="ITestOutputHelper"/> to use.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MSBuildTestProject(ITestOutputHelper testOutputHelper)
        {
            m_TestOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
        }


        /// <summary>
        /// Adds a <c><![CDATA[<Import />]]></c> at the start of the project file
        /// </summary>
        public MSBuildTestProject AddPropertiesImport(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Value must not be null or whitespace", nameof(path));


            m_PropertyImports.Add(path);
            return this;
        }

        /// <summary>
        /// Adds a <c><![CDATA[<Import />]]></c> at the end of the project file
        /// </summary>
        public MSBuildTestProject AddTargetsImport(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Value must not be null or whitespace", nameof(path));


            m_TargetImports.Add(path);
            return this;
        }

        /// <summary>
        /// Adds a file to the project directory
        /// </summary>
        public MSBuildTestProject AddFile(string relativePath, string content)
        {
            if (String.IsNullOrWhiteSpace(relativePath))
                throw new ArgumentException("Value must not be null or whitespace", nameof(relativePath));

            if (Path.IsPathRooted(relativePath))
                throw new ArgumentException("Value must not be a relative path", nameof(relativePath));

            m_Files.Add(relativePath, content);
            return this;
        }

        /// <summary>
        /// Adds a file to the project directory if <paramref name="condition"/> is <c>true</c>.
        /// </summary>
        public MSBuildTestProject AddFileIf(bool condition, string relativePath, string content)
        {
            return condition ? AddFile(relativePath, content) : this;
        }

        /// <summary>
        /// Adds the specifiec NuGet package to the project's NuGet package source and adds a reference to the package to the project file.
        /// </summary>
        /// <param name="packageFilePath">The path of the NuGet package file (.nupkg) to add.</param>
        public MSBuildTestProject InstallNuGetPackage(string packageFilePath)
        {
            if (String.IsNullOrWhiteSpace(packageFilePath))
                throw new ArgumentException("Value must not be null or whitespace", nameof(packageFilePath));

            using var packageReader = new PackageArchiveReader(packageFilePath);

            var packageIdentity = packageReader.GetIdentity();
            m_NuGetPackages.Add((id: packageIdentity.Id, version: packageIdentity.Version.ToString(), packageFilePath));

            return this;
        }

        /// <summary>
        /// Allows updating the MdDocs-specific settings for the project
        /// </summary>
        public MSBuildTestProject ConfigureMdDocs(Action<MdDocsProjectSettings> configure)
        {
            if (configure is null)
                throw new ArgumentNullException(nameof(configure));

            configure(m_MdDocsSettings);
            return this;
        }

        /// <summary>
        /// Saves the test project to the file system
        /// </summary>
        public MSBuildTestProject Save()
        {
            GenerateProjectFile();
            GenerateNuGetConfig();

            // Save all files to disk
            foreach (var (relativePath, content) in m_Files)
            {
                var absolutePath = ProjectDirectory.PathCombine(relativePath);

                if (!absolutePath.StartsWith(m_WorkingDirectory, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Cannot save file outside the working directory");

                Directory.CreateDirectory(absolutePath.GetDirectoryName());
                File.WriteAllText(absolutePath, content);
            }

            // Add NuGet packages to local package source
            foreach (var (id, version, sourcePath) in m_NuGetPackages)
            {
                var destinationPath = LocalNuGetPackageSourcePath.PathCombine($"{id}.{version}.nupkg");

                Directory.CreateDirectory(LocalNuGetPackageSourcePath);
                if (!File.Exists(destinationPath))
                {
                    File.Copy(sourcePath, destinationPath);
                }
            }

            return this;
        }

        /// <summary>
        /// Builds the project using the specified version of MSBuild.
        /// </summary>
        public MSBuildExecutionResult Build(MSBuildRuntimeInfo runtime, string? msbuildArgs = null)
        {
            Save();

            return runtime.Type switch
            {
                MSBuildRuntimeType.Full => RunMSBuild(runtime.Version, msbuildArgs),
                MSBuildRuntimeType.Core => RunDotnetBuild(runtime.Version, msbuildArgs),
                _ => throw new NotImplementedException()
            };
        }

        /// <inheritdoc />
        public void Dispose()
        {
            m_WorkingDirectory.Dispose();
        }


        private void GenerateProjectFile()
        {
            var root = new XElement(
                "Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                new XElement("PropertyGroup"),
                new XElement("ItemGroup")
            );


            void AddProperty(string name, string value)
            {
                var propertyGroup = root.Elements("PropertyGroup").Single();
                propertyGroup.Add(new XElement(name, value));
            }

            void AddOptionalProperty(string name, string? value)
            {
                if (value is not null)
                {
                    var propertyGroup = root.Elements("PropertyGroup").Single();
                    propertyGroup.Add(new XElement(name, value));
                }
            }

            void AddOptionalBoolProperty(string name, bool? value)
            {
                if (value is not null)
                {
                    var propertyGroup = root.Elements("PropertyGroup").Single();
                    propertyGroup.Add(new XElement(name, value.Value.ToString().ToLower()));
                }
            }

            void AddPackageReference(string id, string version)
            {
                var itemGroup = root.Elements("ItemGroup").Single();
                itemGroup.Add(new XElement(
                    "PackageReference",
                    new XAttribute("Include", id),
                    new XAttribute("Version", version)
                ));
            }

            // Add imports of .props files
            foreach (var import in m_PropertyImports)
            {
                var importElement = new XElement("Import", new XAttribute("Project", import));
                root.Elements().First().AddBeforeSelf(importElement);
            }

            // Add project settings
            AddProperty("TargetFramework", s_TargetFramework);
            AddProperty("IsPackable", "false");
            AddProperty("OutputPath", $"{RootOutputDirectory.TrimEndingDirectorySeparator()}\\$(Configuration)\\");

            //
            // If set, save MdDocs settings as properties
            //
            AddOptionalProperty("ApiReferenceDocumentationOutputPath", m_MdDocsSettings.ApiReferenceDocumentationOutputPath);
            AddOptionalProperty("CommandLineDocumentationOutputPath", m_MdDocsSettings.CommandLineDocumentationOutputPath);
            AddOptionalProperty("MdDocsConfigurationFilePath", m_MdDocsSettings.ConfigurationFilePath);
            AddOptionalBoolProperty("GenerateApiReferenceDocumentationOnBuild", m_MdDocsSettings.GenerateApiReferenceDocumentationOnBuild);
            AddOptionalBoolProperty("GenerateCommandLineDocumentationOnBuild", m_MdDocsSettings.GenerateCommandLineDocumentationOnBuild);

            // Add PackageReferences
            foreach (var (id, version, _) in m_NuGetPackages)
            {
                AddPackageReference(id, version);
            }

            // Add imports of .targets files
            foreach (var import in m_TargetImports)
            {
                var importElement = new XElement("Import", new XAttribute("Project", import));
                root.Elements().Last().AddAfterSelf(importElement);
            }

            m_Files[s_ProjectFileRelativePath] = root.ToString();
        }

        private void GenerateNuGetConfig()
        {
            m_Files["../nuget.config"] = $"""
                <?xml version="1.0" encoding="utf-8"?>
                <configuration>
                  <packageSources>
                    <clear />
                    <add key="nuget" value="https://api.nuget.org/v3/index.json" />
                    <add key="local" value="{LocalNuGetPackageSourcePath}" />    
                  </packageSources>
                </configuration>                
                """;
        }

        private MSBuildExecutionResult RunMSBuild(Version msbuildVersion, string? args)
        {
            var msbuildPath = VSWhere.GetMSBuildPath(msbuildVersion.Major);
            var binaryLogFilePath = RootOutputDirectory.PathCombine($"{Guid.NewGuid()}.binlog");
            var exitCode = Exec(msbuildPath, $"\"{ProjectPath}\" {args ?? ""} /restore \"/binaryLogger:LogFile={binaryLogFilePath}\"");

            return new()
            {
                ExitCode = exitCode,
                BinaryLogFilePath = binaryLogFilePath
            };
        }

        private MSBuildExecutionResult RunDotnetBuild(Version dotnetSdkVersion, string? args)
        {
            File.WriteAllText(m_WorkingDirectory.PathCombine("global.json"),
               $@"{{
                    ""sdk"": {{
                        ""version"": ""{dotnetSdkVersion}""
                    }}
                }}");

            var binaryLogFilePath = RootOutputDirectory.PathCombine($"{Guid.NewGuid()}.binlog");
            var exitCode = Exec("dotnet", $"build \"{ProjectPath}\" {args ?? ""} \"/binaryLogger:LogFile={binaryLogFilePath}\"");
            return new()
            {
                ExitCode = exitCode,
                BinaryLogFilePath = binaryLogFilePath
            };
        }

        private int Exec(string fileName, string arguments)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = fileName,
                Arguments = arguments,
                WorkingDirectory = ProjectDirectory,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            // The build process inherits the environment variables of the test process.
            // Because the test process is itself running on .NET,
            // some of the inherited environment varibales cause errors,
            // especially when the test uses a different .NET Core SDK than the test process.
            // To avoid these errors, remove environment variables that would not be set
            // when a process is started from the commandline from the child process.
            startInfo.EnvironmentVariables.Remove("DOTNET_CLI_TELEMETRY_SESSIONID");
            startInfo.EnvironmentVariables.Remove("DOTNET_HOST_PATH");
            startInfo.EnvironmentVariables.Remove("MSBUILDENSURESTDOUTFORTASKPROCESSES");
            startInfo.EnvironmentVariables.Remove("MSBuildExtensionsPath");
            startInfo.EnvironmentVariables.Remove("MSBuildLoadMicrosoftTargetsReadOnly");
            startInfo.EnvironmentVariables.Remove("MSBuildSDKsPath");

            // use a test-specific nuget cache path
            startInfo.EnvironmentVariables.Add("NUGET_PACKAGES", NuGetCachePath);

            var process = Process.Start(startInfo);

            if (process is null)
                throw new Exception($"Failed to start '{startInfo.FileName}'");

            process.OutputDataReceived += (s, e) => m_TestOutputHelper.WriteLine(e?.Data ?? "");
            process.ErrorDataReceived += (s, e) => m_TestOutputHelper.WriteLine(e?.Data ?? "");

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            process.CancelErrorRead();
            process.CancelOutputRead();

            return process.ExitCode;
        }
    }
}
