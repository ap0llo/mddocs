using Grynwald.MdDocs.CommandLineHelp.Model;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    /// <summary>
    /// Tests for <see cref="MultiCommandApplicationDocumentation"/>
    /// </summary>
    public class MultiCommandApplicationDocumentationTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData("  ")]
        public void AddCommand_throws_InvaldModelException_if_command_name_is_null_or_whitespace(string commandName)
        {
            // ARRANGE
            var sut = new MultiCommandApplicationDocumentation("app", "1.0");

            // ACT / ASSERT
            Assert.Throws<InvalidModelException>(() => sut.AddCommand(commandName));
        }

        [Theory]
        [InlineData("name", "name")]
        [InlineData("name", "NAME")]
        [InlineData("NaMe", "name")]
        public void AddCommand_throws_InvaldModelException_if_command_with_the_same_name_already_exists(string commandName1, string commandName2)
        {
            // ARRANGE
            var sut = new MultiCommandApplicationDocumentation("app", "1.0");
            _ = sut.AddCommand(commandName1);

            // ACT / ASSERT
            Assert.Throws<InvalidModelException>(() => sut.AddCommand(commandName2));
        }


    }
}
