using System;
using System.Linq;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.TestData;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    public class CommandDocumentationTest : CommandLineDynamicCompilationTestBase
    {
        private CommandDocumentation GetCommandDocumentation(TypeDefinition typeDefinition)
        {
            return CommandDocumentation.FromTypeDefinition(new MultiCommandApplicationDocumentation("test"), typeDefinition, NullLogger.Instance);
        }
        
        [Fact]        
        public void Options_returns_expected_number_of_items_01()
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

                [Verb(""command"")]
                public class CommandOptions
                {
                    [Option(""option1"")]
                    public string Option1Property { get; set; }

                    [Option('x')]
                    public string Option2Property { get; set; }

                    [Option('y')]
                    public string Option3Property { get; set; }

                    [Option(""option4"", Hidden = true)]
                    public string Option4Property { get; set; }

                    [Option(""option5"")]
                    public SomeEnum Option5Property { get; set; }

                    [Option(""option6"")]
                    public SomeEnum Option6Property { get; set; }
                }
            ";

            using var assembly = Compile(cs);

            var optionType = assembly.MainModule.Types.Single(x => x.Name == "CommandOptions");

            // ACT
            var sut = GetCommandDocumentation(optionType);

            // ASSERT
            // Options must not return hidden items
            Assert.Equal(5, sut.Options.Count);
        }

        [Fact]
        public void Options_returns_expected_number_of_items_02()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;

                public abstract class OptionsBase
                {
                    [Option(""parameterFromBaseClass"")]
                    public int ParameterFromBaseClass { get; set; }

                    [Option(""abstractParameter"")]
                    public abstract int AbstractParameter { get; set; }
                }

                [Verb(""command"", HelpText = ""Some Help Text"")]
                public class CommandOptions : OptionsBase
                {
                    public override int AbstractParameter { get; set; }
                }
            ";

            using var assembly = Compile(cs);

            var optionType = assembly.MainModule.Types.Single(x => x.Name == "CommandOptions");

            // ACT
            var sut = GetCommandDocumentation(optionType);

            // ASSERT
            Assert.Equal(2, sut.Options.Count);
        }

        [Fact]
        public void Commands_have_the_expected_properties()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;
               
                [Verb(""command1"", HelpText = ""Some Help Text"")]
                public class Command1Options
                {
                }

                [Verb(""command2"", HelpText = ""Another command"", Hidden = true)]
                public class Command2Options
                {
                }
	
	            [Verb(""command3"")]
                public class Command3Options
                {
                }
            ";

            using var assembly = Compile(cs);

            {
                // ACT
                var command = GetCommandDocumentation(assembly.MainModule.Types.Single(x => x.Name == "Command1Options"));

                // ASSERT
                Assert.Equal("command1", command.Name);
                Assert.Equal("Some Help Text", command.HelpText);
                Assert.False(command.Hidden);
            }
            {
                // ACT
                var command = GetCommandDocumentation(assembly.MainModule.Types.Single(x => x.Name == "Command2Options"));

                // ASSERT
                Assert.Equal("command2", command.Name);
                Assert.Equal("Another command", command.HelpText);
                Assert.True(command.Hidden);
            }
            {
                // ACT
                var command = GetCommandDocumentation(assembly.MainModule.Types.Single(x => x.Name == "Command3Options"));

                // ASSERT
                Assert.Equal("command3", command.Name);
                Assert.Null(command.HelpText);
                Assert.False(command.Hidden);
            }

            
        }

        [Fact]
        public void Expected_option_name_exist()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;
               
                [Verb(""command"")]
                public class CommandOptions
                {
                    [Option(""option1"")]
                    public string Option1Property { get; set; }

                    [Option('x')]
                    public string Option2Property { get; set; }

                    [Option('y', ""option3"", Required = true)]
                    public string Option3Property { get; set; }
                }
            ";

            using var assembly = Compile(cs);

            // ACT
            var command = GetCommandDocumentation(assembly.MainModule.Types.Single(x => x.Name == "CommandOptions"));

            // ASSERT            
            Assert.Contains(command.Options, o => o.Name == "option1");
            Assert.Contains(command.Options, o => o.Name == "option3");
        }

        [Fact]
        public void Hidden_options_are_ignored()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;
               
                [Verb(""command"")]
                public class CommandOptions
                {
                    [Option(""option1"")]
                    public string Option1Property { get; set; }
                    
                    [Option('y', ""option2"", Hidden = true)]
                    public string Option2Property { get; set; }
                }
            ";


            using var assembly = Compile(cs);

            // ACT
            var command = GetCommandDocumentation(assembly.MainModule.Types.Single(x => x.Name == "CommandOptions"));

            // ASSERT            
            Assert.DoesNotContain(command.Options, o => o.Name == "option2");
        }

        [Fact]
        public void Expected_option_short_name_exists()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;
               
                [Verb(""command"")]
                public class CommandOptions
                {
                    [Option('x')]
                    public string Option1Property { get; set; }

                    [Option('y', ""option2"", Required = true)]
                    public string Option2Property { get; set; }
                }
            ";

            using var assembly = Compile(cs);

            // ACT
            var command = GetCommandDocumentation(assembly.MainModule.Types.Single(x => x.Name == "CommandOptions"));

            // ASSERT
            Assert.Contains(command.Options, o => o.ShortName == 'x');
            Assert.Contains(command.Options, o => o.ShortName == 'y');
        }

        [Fact]
        public void Expected_value_exists()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;
               
                [Verb(""command"")]
                public class CommandOptions
                {
                    [Value(0)]
                    public string Value1 { get; set; }

                    [Value(1, MetaName = ""Value2 name"")]
                    public string Value2 { get; set; }
                }
            ";

            using var assembly = Compile(cs);

            // ACT
            var command = GetCommandDocumentation(assembly.MainModule.Types.Single(x => x.Name == "CommandOptions"));

            // ASSERT
            Assert.Contains(command.Values, o => o.Index == 0);
            Assert.Contains(command.Values, o => o.Index == 1);
        }
    }
}
