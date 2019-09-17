using System;
using System.Linq;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.TestData;
using Grynwald.MdDocs.CommandLineHelp.TestData.SingleCommandApp;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    public class UnnamedCommandDocumentationTest
    {
        private TypeDefinition GetTypeDefinition(Type optionsType)
        {
            var assemblyPath = optionsType.Assembly.Location;
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyPath);

            return assemblyDefinition.MainModule.Types.Single(t => t.FullName == optionsType.FullName);
        }

        private UnnamedCommandDocumentation LoadDocumentation(Type optionsType)
        {
            var definition = GetTypeDefinition(optionsType);
            return UnnamedCommandDocumentation.FromTypeDefinition(new MultiCommandApplicationDocumentation("Test"), definition, NullLogger.Instance);
        }


        [Fact]
        public void FromTypeDefinition_throws_ArgumentException_for_type_definitions_with_Verb_attributes()
        {
            var application = new MultiCommandApplicationDocumentation("Test");
            var definition = GetTypeDefinition(typeof(Command1Options));

            Assert.Throws<ArgumentException>(() => UnnamedCommandDocumentation.FromTypeDefinition(application, definition, NullLogger.Instance));
        }

        [Theory]
        [InlineData(typeof(Options), "option1")]
        [InlineData(typeof(Options), "option2")]
        public void Expected_option_name_exists(Type optionsType, string optionName)
        {
            var command = LoadDocumentation(optionsType);
            Assert.Contains(command.Options, o => o.Name == optionName);
        }

        [Theory]
        [InlineData(typeof(Options), "option3")]
        public void Hiiden_options_are_ignored(Type optionsType, string optionName)
        {
            var command = LoadDocumentation(optionsType);
            Assert.DoesNotContain(command.Options, o => o.Name == optionName);
        }

        [Theory]
        [InlineData(typeof(Options), 'x')]
        [InlineData(typeof(Options), 'y')]
        public void Expected_option_short_name_exists(Type optionsType, char shortName)
        {
            var command = LoadDocumentation(optionsType);
            Assert.Contains(command.Options, o => o.ShortName == shortName);
        }

        [Theory]
        [InlineData(typeof(Options), 0)]
        [InlineData(typeof(Options), 1)]
        public void Expected_value_exists(Type optionsType, int index)
        {
            var command = LoadDocumentation(optionsType);
            Assert.Contains(command.Values, o => o.Index == index);
        }
    }
}
