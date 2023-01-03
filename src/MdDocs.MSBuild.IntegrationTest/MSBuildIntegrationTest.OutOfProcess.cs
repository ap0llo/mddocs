using System.IO;
using Xunit;

namespace Grynwald.MdDocs.MSBuild.IntegrationTest
{
    public partial class MSBuildIntegrationTest
    {
        [Theory]
        [MemberData(nameof(MSBuildRuntimesData))]
        public void MdDocs_is_started_to_generate_Api_Reference_during_build(MSBuildRuntimeInfo msbuildRuntime)
        {
            // ARRANGE
            var package = m_PackagesFixture.GetPackage("Grynwald.MdDocs.MSBuild");

            using var project = new MSBuildTestProject(m_OutputHelper);

            // Paths
            var configuredApiReferenceOutputDirectory = "api/";
            var absoluteApiReferenceOutputDirectory = project.ProjectDirectory.PathCombine(configuredApiReferenceOutputDirectory).GetFullPath().TrimEndingDirectorySeparator();
            var mddocsCliAssemblyPath = Path.Combine(project.NuGetCachePath, package.Id.ToLowerInvariant(), package.Identity.Version.ToString(), "tools", "net6.0", "mddocs.dll");

            // Expected arguments
            var expectedArguments = new ProcessArgumentsBuilder()
                .Append("dotnet").AppendQuoted(mddocsCliAssemblyPath)
                .Append("apireference")
                .Append("--assemblies").AppendQuoted(project.TargetPath)
                .Append("--outdir").AppendQuoted(absoluteApiReferenceOutputDirectory);

            // Set up project
            project
                .InstallNuGetPackage(package.PackageFilePath)
                .ConfigureMdDocs(x =>
                {
                    x.ApiReferenceDocumentationOutputPath = configuredApiReferenceOutputDirectory;
                    x.GenerateApiReferenceDocumentationOnBuild = true;
                });

            // ACT
            var result = project.Build(msbuildRuntime);

            // ASSERT
            Assert.Equal(0, result.ExitCode);

            // Find the log of the "Exec" task inside the "GenerateApiReferenceDocumentation" target
            var target = Assert.Single(result.BinaryLog.GetTargets("GenerateApiReferenceDocumentation"));
            var execTask = Assert.Single(target.GetTasks("Exec"));
            Assert.Equal(expectedArguments.ToString(), execTask.CommandLineArguments);
        }
    }
}
