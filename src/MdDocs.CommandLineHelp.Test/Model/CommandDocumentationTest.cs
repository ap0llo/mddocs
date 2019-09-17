using System;
using System.Linq;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.TestData;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    public class CommandDocumentationTest
    {
        private CommandDocumentation LoadDocumentation(Type optionsType)
        {
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(optionsType.Assembly.Location);

            var typeDefinition = assemblyDefinition.MainModule.Types.Single(t => t.FullName == optionsType.FullName);

            return CommandDocumentation.FromTypeDefinition(new MultiCommandApplicationDocumentation("test"), typeDefinition, NullLogger.Instance);
        }


        [Fact]
        public void Options_returns_expected_number_of_items()
        {
            var sut = LoadDocumentation(typeof(Command3Options));

            // Options does not return hidden items
            Assert.Equal(4, sut.Options.Count);
        }

        [Theory]
        [InlineData(typeof(Command1Options), "command1", "Some Help Text", false)]
        [InlineData(typeof(Command2Options), "command2", "Another command", true)]
        [InlineData(typeof(Command3Options), "command3", null, false)]
        public void Commands_have_the_expected_properties(Type optionsType, string name, string helpText, bool hidden)
        {
            var command = LoadDocumentation(optionsType);
                        
            Assert.Equal(name, command.Name);
            Assert.Equal(helpText, command.HelpText);
            Assert.Equal(hidden, command.Hidden);
        }

        [Theory]
        [InlineData(typeof(Command3Options), "option1")]
        [InlineData(typeof(Command3Options), "option3")]
        public void Expected_option_name_exists(Type optionsType, string optionName)
        {
            var command = LoadDocumentation(optionsType);
            Assert.Contains(command.Options, o => o.Name == optionName);
        }

        [Theory]
        [InlineData(typeof(Command3Options), "option4")]
        public void Hiiden_options_are_ignored(Type optionsType, string optionName)
        {
            var command = LoadDocumentation(optionsType);
            Assert.DoesNotContain(command.Options, o => o.Name == optionName);
        }


        [Theory]
        [InlineData(typeof(Command3Options), 'x')]
        [InlineData(typeof(Command3Options), 'y')]
        public void Expected_option_short_name_exists(Type optionsType, char shortName)
        {
            var command = LoadDocumentation(optionsType);
            Assert.Contains(command.Options, o => o.ShortName == shortName);
        }

        [Theory]
        [InlineData(typeof(Command4Options), 0)]
        [InlineData(typeof(Command4Options), 1)]        
        public void Expected_value_exists (Type optionsType, int index)
        {
            var command = LoadDocumentation(optionsType);
            Assert.Contains(command.Values, o => o.Index== index);
        }
    }
}
