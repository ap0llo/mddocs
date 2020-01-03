using System.Linq;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.TestData;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    public class OptionDocumentationTest : CommandLineDynamicCompilationTestBase
    {
        [Theory]
        [InlineData(@"[Option(""option1"")]", "option1", null, false, false, null, null)]
        [InlineData(@"[Option('x')]", null, 'x', false, false, null, null)]
        [InlineData(@"[Option('y', ""option3"", Required = true)]", "option3", 'y', true, false, null, null)]
        [InlineData(@"[Option(""option4"", HelpText = ""Option 4 Help text"", Hidden = true, Default = ""DefaultValue"")]", "option4", null, false, true, "Option 4 Help text", "DefaultValue")]
        public void Option_has_the_expected_properties(string optionAttribute, string name, char? shortName, bool required, bool hidden, string helpText, object defaultValue)
        {            
            // ARRANGE
            var cs = $@"
                using CommandLine;
             
                public class Options
                {{
                    {optionAttribute}
                    public string Option1Property {{ get; set; }}
                }}
            ";

            using var assembly = Compile(cs);

            // ACT            
            var sut = OptionDocumentation.FromPropertyDefinition(assembly.MainModule.Types.Single(x => x.Name == "Options").Properties.Single(), NullLogger.Instance);

            // ASSERT
            Assert.Equal(name, sut.Name);
            Assert.Equal(shortName, sut.ShortName);
            Assert.Equal(required, sut.Required);
            Assert.Equal(hidden, sut.Hidden);
            Assert.Equal(helpText, sut.HelpText);
            Assert.Equal(defaultValue, sut.Default);
        }

        [Fact]
        public void AcceptedValues_is_loaded_correctly_for_enum_types()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;

                public enum SomeEnum
                {
                    Value1,
                    Value2,
                    SomeOtherValue
                }

                public class Options
                {
                    [Option(""option1"")]
                    public SomeEnum Option1Property { get; set; }
                }
            ";

            using var assembly = Compile(cs);

            // ACT            
            var sut = OptionDocumentation.FromPropertyDefinition(assembly.MainModule.Types.Single(x => x.Name == "Options").Properties.Single(), NullLogger.Instance);

            // ASSERT
            Assert.NotNull(sut.AcceptedValues);
            Assert.Equal(3, sut.AcceptedValues!.Count);
            foreach (var value in new[] { "Value1", "Value2", "SomeOtherValue" })
            {
                Assert.Contains(value, sut.AcceptedValues);
            }
        }

        [Fact]
        public void Default_of_enum_options_gets_the_enum_constant_as_string()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;

                public enum SomeEnum
                {
                    Value1,
                    Value2,
                    SomeOtherValue
                }

                public class Options
                {
                    [Option(""option1"", Default = SomeEnum.SomeOtherValue)]
                    public SomeEnum Option1Property { get; set; }
                }
            ";

            using var assembly = Compile(cs);

            // ACT            
            var sut = OptionDocumentation.FromPropertyDefinition(assembly.MainModule.Types.Single(x => x.Name == "Options").Properties.Single(), NullLogger.Instance);

            // ASSERT
            Assert.NotNull(sut.Default);
            Assert.Equal("SomeOtherValue", sut.Default);
        }
    }
}
