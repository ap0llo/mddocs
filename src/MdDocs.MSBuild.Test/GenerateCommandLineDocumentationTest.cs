using System.IO;
using Microsoft.Build.Utilities;
using Xunit;

namespace Grynwald.MdDocs.MSBuild.Test
{
    public class GenerateCommandLineDocumentationTest
    {
        [Fact]
        public void AssemblyPath_returns_full_path()
        {
            // ARRANGE
            var sut = new GenerateCommandLineDocumentation()
            {
                Assembly = new TaskItem("my-assembly.dll"),
                BuildEngine = new BuildEngineMock()
            };

            var expectedPath = Path.GetFullPath("my-assembly.dll");

            // ACT 
            var actualPath = sut.AssemblyPath;

            // ASSERT
            Assert.True(Path.IsPathRooted(actualPath));
            Assert.Equal(expectedPath, actualPath);
        }

        [Fact]
        public void AssemblyPath_overrides_assembly_path_setting()
        {
            // ARRANGE
            var sut = new GenerateCommandLineDocumentation()
            {
                Assembly = new TaskItem("my-assembly.dll"),
                BuildEngine = new BuildEngineMock()
            };

            var expectedPath = Path.GetFullPath("my-assembly.dll");

            // ACT 
            var config = sut.LoadConfiguration();

            // ASSERT
            Assert.Equal(expectedPath, config.CommandLineHelp.AssemblyPath);
        }

        [Fact]
        public void OutputDirectoryPath_returns_empty_string_if_OutputDirectory_is_null()
        {
            // ARRANGE
            var sut = new GenerateCommandLineDocumentation()
            {
                Assembly = new TaskItem("my-assembly.dll"),
                BuildEngine = new BuildEngineMock(),
                OutputDirectory = null
            };

            // ACT 
            var outputDirectoryPath = sut.OutputDirectoryPath;

            // ASSERT
            Assert.NotNull(outputDirectoryPath);
            Assert.Equal("", outputDirectoryPath);
        }

        [Fact]
        public void OutputDirectoryPath_overrides_output_path_setting()
        {
            // ARRANGE
            var sut = new GenerateCommandLineDocumentation()
            {
                Assembly = new TaskItem("my-assembly.dll"),
                BuildEngine = new BuildEngineMock(),
                OutputDirectory = new TaskItem("some-output-directory")
            };

            var expectedOutputPath = Path.GetFullPath("some-output-directory");

            // ACT
            var config = sut.LoadConfiguration();

            // ASSERT            
            Assert.Equal(expectedOutputPath, config.CommandLineHelp.OutputPath);
        }

        [Theory]
        [CombinatorialData]
        public void IncludeVersion_overrides_include_version_setting(bool includeVersion)
        {
            // ARRANGE
            var sut = new GenerateCommandLineDocumentation()
            {
                Assembly = new TaskItem("my-assembly.dll"),
                BuildEngine = new BuildEngineMock(),
                IncludeVersion = includeVersion
            };


            // ACT
            var config = sut.LoadConfiguration();

            // ASSERT            
            Assert.Equal(includeVersion, config.CommandLineHelp.IncludeVersion);
        }
    }
}
