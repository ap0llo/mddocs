using System.IO;
using Xunit;

namespace Grynwald.MdDocs.Test
{
    /// <summary>
    /// Tests for <see cref="CommandLineHelpOptions"/>
    /// </summary>
    public class CommandLineHelpOptionsTest
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(false, null)]
        public void IncludeVersion_returns_expected_value_based_on_the_no_version_flag(bool noVersion, bool? expected)
        {
            // ARRANGE
            var sut = new CommandLineHelpOptions()
            {
                NoVersion = noVersion
            };

            // ACT 
            var actual = sut.IncludeVersion;

            // ASSERT
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OutputDirectory_converts_value_to_a_absolute_path()
        {
            // ARRANGE
            var sut = new CommandLineHelpOptions()
            {
                OutputDirectory = "some-path"
            };

            // ACT / ASSERT
            Assert.True(Path.IsPathRooted(sut.OutputDirectory));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        public void OutputDirectory_does_not_convert_value_to_a_absolute_path_if_path_is_null_or_empty(string path)
        {
            // ARRANGE
            var sut = new CommandLineHelpOptions()
            {
                OutputDirectory = path
            };

            // ACT / ASSERT
            Assert.Equal(path, sut.OutputDirectory);
        }


        [Fact]
        public void AssemblyPath_converts_value_to_a_absolute_path()
        {
            // ARRANGE
            var sut = new CommandLineHelpOptions()
            {
                AssemblyPath = "some-path"
            };

            // ACT / ASSERT
            Assert.True(Path.IsPathRooted(sut.AssemblyPath));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        public void AssemblyPath_does_not_convert_value_to_a_absolute_path_if_path_is_null_or_empty(string path)
        {
            // ARRANGE
            var sut = new CommandLineHelpOptions()
            {
                AssemblyPath = path
            };

            // ACT / ASSERT
            Assert.Equal(path, sut.AssemblyPath);
        }
    }
}
