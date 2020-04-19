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
            var sut = new ApiReferenceOptions
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
            var sut = new ApiReferenceOptions
            {
                OutputDirectory = path
            };

            // ACT / ASSERT
            Assert.Equal(path, sut.OutputDirectory);
        }
    }
}
