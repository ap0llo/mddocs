using Grynwald.MdDocs.CommandLineHelp.Model;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    /// <summary>
    /// Tests for <see cref="ParameterCollection"/>
    /// </summary>
    public class ParameterCollectionTest
    {
        private readonly ApplicationDocumentation m_ApplicationDocumentation = new SingleCommandApplicationDocumentation("app", "1.0");


        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("\t", "")]
        [InlineData("", "\t")]
        [InlineData("  ", "\t")]
        [InlineData("  ", null)]
        public void AddNamedParameter_throws_InvalidModelException_if_both_name_and_short_name_are_null_or_whitespace(string parameterName, string parameterShortName)
        {
            // ARRANGE
            var sut = new ParameterCollection(m_ApplicationDocumentation, null);

            // ACT / ASSERT
            Assert.Throws<InvalidModelException>(() => sut.AddNamedParameter(parameterName, parameterShortName));
        }

        [Theory]
        [InlineData("parameter1", null, "parameter1", null)]
        [InlineData("parameter1", "x", "parameter1", "x")]
        [InlineData(null, "x", null, "x")]
        // parameter names must be treated case-insensitive
        [InlineData("parameter1", null, "PARAMETER1", null)]
        [InlineData("parameter1", "x", "PARAMETER2", "X")]
        [InlineData(null, "x", null, "X")]
        public void AddNamedParameter_throws_InvalidModelException_if_parameter_with_the_same_name_already_exists(string parameterName1, string parameterShortName1, string parameterName2, string parameterShortName2)
        {
            // ARRANGE
            var sut = new ParameterCollection(m_ApplicationDocumentation, null);
            _ = sut.AddNamedParameter(parameterName1, parameterShortName1);

            // ACT / ASSERT
            Assert.Throws<InvalidModelException>(() => sut.AddNamedParameter(parameterName2, parameterShortName2));
        }

        [Theory]
        [InlineData("parameter1", null, "parameter1", null)]
        [InlineData("parameter1", "x", "parameter1", "x")]
        [InlineData(null, "x", null, "x")]
        // parameter names must be treated case-insensitive
        [InlineData("parameter1", null, "PARAMETER1", null)]
        [InlineData("parameter1", "x", "PARAMETER2", "X")]
        [InlineData(null, "x", null, "X")]
        public void AddNamedParameter_throws_InvalidModelException_if_a_switch_parameter_with_the_same_name_already_exists(string parameterName1, string parameterShortName1, string parameterName2, string parameterShortName2)
        {
            // ARRANGE
            var sut = new ParameterCollection(m_ApplicationDocumentation, null);
            _ = sut.AddSwitchParameter(parameterName1, parameterShortName1);

            // ACT / ASSERT
            Assert.Throws<InvalidModelException>(() => sut.AddNamedParameter(parameterName2, parameterShortName2));
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("\t", "")]
        [InlineData("", "\t")]
        [InlineData("  ", "\t")]
        [InlineData("  ", null)]
        public void AddSwitchParameter_throws_InvalidModelException_if_both_name_and_short_name_are_null_or_whitespace(string parameterName, string parameterShortName)
        {
            // ARRANGE
            var sut = new ParameterCollection(m_ApplicationDocumentation, null);

            // ACT / ASSERT
            Assert.Throws<InvalidModelException>(() => sut.AddSwitchParameter(parameterName, parameterShortName));
        }

        [Theory]
        [InlineData("parameter1", null, "parameter1", null)]
        [InlineData("parameter1", "x", "parameter1", "x")]
        [InlineData(null, "x", null, "x")]
        // parameter names must be treated case-insensitive
        [InlineData("parameter1", null, "PARAMETER1", null)]
        [InlineData("parameter1", "x", "PARAMETER2", "X")]
        [InlineData(null, "x", null, "X")]
        public void AddSwitchParameter_throws_InvalidModelException_if_parameter_with_the_same_name_already_exists(string parameterName1, string parameterShortName1, string parameterName2, string parameterShortName2)
        {
            // ARRANGE
            var sut = new ParameterCollection(m_ApplicationDocumentation, null);
            _ = sut.AddSwitchParameter(parameterName1, parameterShortName1);

            // ACT / ASSERT
            Assert.Throws<InvalidModelException>(() => sut.AddSwitchParameter(parameterName2, parameterShortName2));
        }

        [Theory]
        [InlineData("parameter1", null, "parameter1", null)]
        [InlineData("parameter1", "x", "parameter1", "x")]
        [InlineData(null, "x", null, "x")]
        // parameter names must be treated case-insensitive
        [InlineData("parameter1", null, "PARAMETER1", null)]
        [InlineData("parameter1", "x", "PARAMETER2", "X")]
        [InlineData(null, "x", null, "X")]
        public void AddSwitchParameter_throws_InvalidModelException_if_a_named_parameter_with_the_same_name_already_exists(string parameterName1, string parameterShortName1, string parameterName2, string parameterShortName2)
        {
            // ARRANGE
            var sut = new ParameterCollection(m_ApplicationDocumentation, null);
            _ = sut.AddNamedParameter(parameterName1, parameterShortName1);

            // ACT / ASSERT
            Assert.Throws<InvalidModelException>(() => sut.AddSwitchParameter(parameterName2, parameterShortName2));
        }

        [Fact]
        public void AddPositionalParameter_throws_InvalidModelException_if_parameter_with_the_same_position_already_exists()
        {
            // ARRANGE
            var sut = new ParameterCollection(m_ApplicationDocumentation, null);
            _ = sut.AddPositionalParameter(1);

            // ACT / ASSERT
            Assert.Throws<InvalidModelException>(() => sut.AddPositionalParameter(1));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-23)]
        public void AddPositionalParameter_throws_InvalidModelException_if_position_is_out_of_range(int position)
        {
            // ARRANGE
            var sut = new ParameterCollection(m_ApplicationDocumentation, null);

            // ACT / ASSERT
            Assert.Throws<InvalidModelException>(() => sut.AddPositionalParameter(position));
        }
    }
}
