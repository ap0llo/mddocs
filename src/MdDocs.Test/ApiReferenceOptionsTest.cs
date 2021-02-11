using System.IO;
using Xunit;

namespace Grynwald.MdDocs.Test
{
    /// <summary>
    /// Tests for <see cref="ApiReferenceOptions"/>
    /// </summary>
    public class ApiReferenceOptionsTest
    {
        [Fact]
        public void OutputDirectory_converts_value_to_a_absolute_path()
        {
            // ARRANGE
            var sut = new ApiReferenceOptions()
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
            var sut = new ApiReferenceOptions()
            {
                OutputDirectory = path
            };

            // ACT / ASSERT
            Assert.Equal(path, sut.OutputDirectory);
        }

        [Fact]
        public void AssemblyPaths_converts_values_to_a_absolute_paths_01()
        {
            // ARRANGE
            var sut = new ApiReferenceOptions()
            {
                AssemblyPaths = new[] { "some-path" }
            };

            // ACT / ASSERT
            Assert.All(sut.AssemblyPaths, path => Assert.True(Path.IsPathRooted(path)));
        }

        [Fact]
        public void AssemblyPaths_converts_values_to_a_absolute_paths_02()
        {
            // ARRANGE
            var sut = new ApiReferenceOptions()
            {
                AssemblyPaths = new[] { "some-path", "some-other-path" }
            };

            // ACT / ASSERT
            Assert.All(sut.AssemblyPaths, path => Assert.True(Path.IsPathRooted(path)));
        }

        [Fact]
        public void AssemblyPaths_does_not_convert_value_to_a_absolute_path_if_value_is_null()
        {
            // ARRANGE
            var sut = new ApiReferenceOptions()
            {
                AssemblyPaths = null
            };

            // ACT / ASSERT
            Assert.Null(sut.AssemblyPaths);
        }
    }
}
