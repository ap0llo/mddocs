using System;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    /// <summary>
    /// Tests for <see cref="NamedParameterDocumentation"/>
    /// </summary>
    public class NamedParameterDocumentationTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        public void Name_and_short_name_must_not_be_both_null_or_empty(string invalidName)
        {
            // ARRANGE
            var application = new MultiCommandApplicationDocumentation("app", "1.2.3");
            var command = application.AddCommand("command");

            // ACT / ASSERT
            Assert.Throws<ArgumentException>(() => new NamedParameterDocumentation(application, command, invalidName, invalidName));
        }
    }
}
