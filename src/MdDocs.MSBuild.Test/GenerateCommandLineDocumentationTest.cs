using System.IO;
using Grynwald.MdDocs.CommandLineHelp.Configuration;
using Grynwald.MdDocs.Common.Configuration;
using Grynwald.Utilities.IO;
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
            var config = sut.GetConfigurationProvider().GetCommandLineHelpConfiguration();

            // ASSERT
            Assert.Equal(expectedPath, config.AssemblyPath);
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
            var config = sut.GetConfigurationProvider().GetCommandLineHelpConfiguration();

            // ASSERT            
            Assert.Equal(expectedOutputPath, config.OutputPath);
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
            var config = sut.GetConfigurationProvider().GetCommandLineHelpConfiguration();

            // ASSERT            
            Assert.Equal(includeVersion, config.IncludeVersion);
        }


        [Theory]
        [CombinatorialData]
        public void MarkdownPreset_property_overrides_configuration_of_markdown_preset(MarkdownPreset preset)
        {
            // ARRANGE
            var sut = new GenerateCommandLineDocumentation()
            {
                Assembly = new TaskItem("myAssembly.dll"),
                BuildEngine = new BuildEngineMock(),
                MarkdownPreset = preset.ToString()
            };

            // ACT 
            var config = sut.GetConfigurationProvider().GetCommandLineHelpConfiguration();

            // ASSERT
            Assert.Equal(preset, config.MarkdownPreset);
        }

        [Theory]
        [CombinatorialData]
        public void LoadConfiguration_file_reads_configuration_file_if_path_is_specified(MarkdownPreset preset)
        {
            // ARRANGE
            using var temporaryDirectory = new TemporaryDirectory();
            var configPath = Path.Combine(temporaryDirectory, "config.json");
            File.WriteAllText(configPath, $@"{{
                ""mddocs"" : {{
                    ""commandlineHelp"" : {{
                        ""markdownPreset"" : ""{preset}""
                    }}
                }}
            }}");

            var sut = new GenerateCommandLineDocumentation()
            {
                Assembly = new TaskItem("myAssembly.dll"),
                BuildEngine = new BuildEngineMock(),
                OutputDirectory = new TaskItem("my-output-directory"),
                ConfigurationFile = new TaskItem(configPath),
                MarkdownPreset = null
            };

            // ACT 
            var config = sut.GetConfigurationProvider().GetCommandLineHelpConfiguration();

            // ASSERT
            Assert.Equal(preset, config.MarkdownPreset);
        }
    }
}
