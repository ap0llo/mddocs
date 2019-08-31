using System;
using System.Collections.Generic;
using Mono.Cecil;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Grynwald.MdDocs.CommandLineHelp.TestData;
using System.Linq;
using Xunit;
using Microsoft.Extensions.Logging.Abstractions;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    public class OptionDocumentationTest
    {
        private OptionDocumentation LoadDocumentation(string propertyName)
        {
            var assemblyPath = typeof(Command3Options).Assembly.Location;
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyPath);

            var typeDefinition = assemblyDefinition
                .MainModule
                .Types
                .Single(t => t.FullName == typeof(Command3Options).FullName);

            var propertyDefinition = typeDefinition
                .Properties
                .Single(p => p.Name == propertyName);

            return OptionDocumentation.FromPropertyDefinition(propertyDefinition, NullLogger.Instance);
        }

        [Theory]
        [InlineData(nameof(Command3Options.Option1Property), "option1", null, false, null, null)]
        [InlineData(nameof(Command3Options.Option2Property), null, 'x', false, null, null)]
        [InlineData(nameof(Command3Options.Option3Property), "option3", 'y', false,  null, null)]
        [InlineData(nameof(Command3Options.Option4Property), "option4", null, true, "Option 4 Help text", "DefaultValue")]
        public void Option_has_the_expected_properties(string propertyName, string name, char? shortName, bool hidden, string helpText, object defaultValue)
        {
            var sut = LoadDocumentation(propertyName);

            Assert.Equal(name, sut.Name);
            Assert.Equal(shortName, sut.ShortName);
            Assert.Equal(hidden, sut.Hidden);
            Assert.Equal(helpText, sut.HelpText);
            Assert.Equal(defaultValue, sut.Default);
        }
    }
}
