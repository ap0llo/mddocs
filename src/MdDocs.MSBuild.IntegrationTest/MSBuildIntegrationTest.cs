using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Grynwald.Utilities.IO;
using NuGet.Common;
using NuGet.Packaging;
using Xunit;
using Xunit.Abstractions;

namespace Grynwald.MdDocs.MSBuild.IntegrationTest
{


    public class MSBuildIntegrationTest : IDisposable
    {
        private readonly TemporaryDirectory m_WorkingDirectory = new TemporaryDirectory();
        private readonly ITestOutputHelper m_OutputHelper;

        public MSBuildIntegrationTest(ITestOutputHelper outputHelper)
        {
            m_OutputHelper = outputHelper;
        }


        public void Dispose() => m_WorkingDirectory.Dispose();


        private static IEnumerable<MSBuildRuntimeInfo> MSBuildRuntimes
        {
            get
            {
                yield return new MSBuildRuntimeInfo(MSBuildRuntimeType.Core, Version.Parse("3.1.100"));
                yield return new MSBuildRuntimeInfo(MSBuildRuntimeType.Core, Version.Parse("2.1.800"));
                yield return new MSBuildRuntimeInfo(MSBuildRuntimeType.Full, Version.Parse("16.0"));
            }
        }

        public static IEnumerable<object[]> TestCases()
        {
            object[] TestCase(MSBuildRuntimeInfo runtime, string msbuildArgs, string[] expectedFiles)
            {
                return new object[] { runtime, msbuildArgs, expectedFiles };
            }

            var apiReferenceExpectedFiles = new[]
            {
                "api/MyNamespace/index.md",
                "api/MyNamespace/Class1/index.md",
                "api/MyNamespace/Class1/constructors/index.md"
            };

            var commandlineHelpExpectedFiles = new[]
            {
                "commandlinehelp/index.md"
            };

            foreach (var runtime in MSBuildRuntimes)
            {
                yield return TestCase(runtime, "/p:GenerateApiReferenceDocumentationOnBuild=true", apiReferenceExpectedFiles);
                yield return TestCase(runtime, "/p:GenerateCommandLineDocumentationOnBuild=true", commandlineHelpExpectedFiles);
            }
        }


        [Theory]
        [MemberData(nameof(TestCases))]
        public void Documentation_is_generated_during_build(MSBuildRuntimeInfo runtime, string msbuildArgs, string[] expectedFiles)
        {
            // ARRANGE
            var packageFilePath = Directory.GetFiles("packages", "*.nupkg").Single();
            var packageId = ExtractNuGetPackage(packageFilePath);

            CreateFile("Class1.cs",
                @"
                namespace MyNamespace
                {
                    public class Class1
                    {
                    }
                }
                ");

            CreateFile("myproject.csproj",
                $@"<Project Sdk=""Microsoft.NET.Sdk"">

                    <Import Project=""$(MSBuildThisFileDirectory)\\{packageId}\\build\\{packageId}.props"" />

                    <PropertyGroup>
                        <TargetFramework>netstandard2.0</TargetFramework>
                        <IsPackable>false</IsPackable>
                        <ApiReferenceDocumentationOutputPath>api/</ApiReferenceDocumentationOutputPath>
                        <CommandLineDocumentationOutputPath>commandlinehelp/</CommandLineDocumentationOutputPath>
                    </PropertyGroup>

                    <Import Project=""$(MSBuildThisFileDirectory)\\{packageId}\\build\\{packageId}.targets"" />

                </Project>");

            // ACT
            var exitCode = RunBuild(runtime, "myproject.csproj", msbuildArgs);

            // ASSERT
            Assert.Equal(0, exitCode);
            foreach (var file in expectedFiles)
            {
                Assert.True(File.Exists(Path.Combine(m_WorkingDirectory, file)));
            }
        }


        private int RunBuild(MSBuildRuntimeInfo runtime, string project, string msbuildArgs)
        {
            return runtime.Type switch
            {
                MSBuildRuntimeType.Full => RunMSBuild(runtime.Version, project, msbuildArgs),
                MSBuildRuntimeType.Core => RunDotnetBuild(runtime.Version, project, msbuildArgs),
                _ => throw new NotImplementedException()
            };
        }

        private void CreateFile(string name, string content)
        {
            var outputPath = Path.Combine(m_WorkingDirectory, name);
            File.WriteAllText(outputPath, content);
        }

        private string ExtractNuGetPackage(string packageFilePath)
        {
            using var stream = File.Open(packageFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new PackageArchiveReader(stream);

            var packageId = reader.GetIdentity().Id;

            foreach (var file in reader.GetFiles())
            {
                var targetPath = Path.Combine(m_WorkingDirectory, packageId, file);
                reader.ExtractFile(file, targetPath, NullLogger.Instance);
            }

            return packageId;
        }

        private int Exec(string fileName, string arguments)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = fileName,
                Arguments = arguments,
                WorkingDirectory = m_WorkingDirectory,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = Process.Start(startInfo);

            process.OutputDataReceived += (s, e) => m_OutputHelper.WriteLine(e?.Data ?? "");
            process.ErrorDataReceived += (s, e) => m_OutputHelper.WriteLine(e?.Data ?? "");

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();


            process.WaitForExit();

            process.CancelErrorRead();
            process.CancelOutputRead();

            return process.ExitCode;
        }

        private int RunMSBuild(Version msbuildVersion, string project, string args)
        {
            var msbuildPath = VSWhere.GetMSBuildPath(msbuildVersion.Major);
            return Exec(msbuildPath, $"{project} {args} /restore");
        }

        private int RunDotnetBuild(Version sdkVersion, string project, string args)
        {
            CreateFile("global.json",
               $@"{{
                    ""sdk"": {{
                        ""version"": ""{sdkVersion}""
                    }}
                }}");

            return Exec("dotnet", $"build {project} {args}");
        }
    }
}
