using System.Linq;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    public class ValueDocumentationTest : CommandLineDynamicCompilationTestBase
    {
        [Theory]
        [InlineData(@"[Value(0)]", 0, false, null, null, false, null)]
        [InlineData(@"[Value(1, MetaName = ""Value2 name"")]", 1, false, "Value2 name", null, false, null)]
        [InlineData(@"[Value(2, HelpText = ""Value 3 Help text"")]", 2, false, null, "Value 3 Help text", false, null)]
        [InlineData(@"[Value(3, Hidden = true, Required = true)]", 3, true, null, null, true, null)]
        [InlineData(@"[Value(4, Default = ""Value 5 Default"")]", 4, false, null, null, false, "Value 5 Default")]
        public void Value_has_the_expected_properties(string valueAttribute, int index, bool required, string name, string helpText, bool hidden, object defaultValue)
        {
            // ARRANGE
            var cs = $@"
                using CommandLine;
             
                public class Options
                {{
                    {valueAttribute}
                    public string Option1Property {{ get; set; }}
                }}
            ";

            using var assembly = Compile(cs);

            // ACT            
            var sut = ValueDocumentation.FromPropertyDefinition(assembly.MainModule.Types.Single(x => x.Name == "Options").Properties.Single(), NullLogger.Instance);

            // ASSERT
            Assert.Equal(index, sut.Index);
            Assert.Equal(name, sut.Name);
            Assert.Equal(required, sut.Required);
            Assert.Equal(helpText, sut.HelpText);
            Assert.Equal(hidden, sut.Hidden);
            Assert.Equal(defaultValue, sut.Default);
        }
    }
}
