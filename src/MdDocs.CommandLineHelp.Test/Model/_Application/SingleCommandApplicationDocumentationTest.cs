using Grynwald.MdDocs.CommandLineHelp.Model;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    public class SingleCommandApplicationDocumentationTest : CommandLineDynamicCompilationTestBase
    {
        [Fact]
        public void Parameters_is_not_null_when_no_option_classes_can_be_found()
        {
            // ARRANGE
            var cs = @"
                public class Class1
                { }
            ";

            using var assembly = Compile(cs);

            // ACT
            var sut = SingleCommandApplicationDocumentation.FromAssemblyDefinition(assembly, NullLogger.Instance);

            // ASSERT
            Assert.NotNull(sut.Command);
            Assert.Empty(sut.Command.Options);
            Assert.Empty(sut.Command.Values);
            Assert.Empty(sut.Command.Parameters);
        }

        [Fact]
        public void Expected_options_exist()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;

                public class Class1
                {
                    [Option(""option1"", Required = true)]
                    public string Option1 { get; set; }

                    [Option(""option2"", Required = true)]
                    public int Option2 { get; set; }

                    [Option(""option3"", Required = true)]
                    public int Option3 { get; set; }
                }
            ";

            using var assembly = Compile(cs);

            // ACT
            var sut = SingleCommandApplicationDocumentation.FromAssemblyDefinition(assembly, NullLogger.Instance);

            // ASSERT
            Assert.Equal(3, sut.Command.Options.Count);
            Assert.Contains(sut.Command.Options, c => c.Name == "option1");
            Assert.Contains(sut.Command.Options, c => c.Name == "option2");
            Assert.Contains(sut.Command.Options, c => c.Name == "option3");
        }

        [Fact]
        public void Hidden_options_are_ignored()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;

                public class Class1
                {
                    [Option(""option1"", Required = true)]
                    public string Option1 { get; set; }

                    [Option(""option2"", Required = true)]
                    public int Option2 { get; set; }

                    [Option(""option3"", Required = true, Hidden = true)]
                    public int Option3 { get; set; }
                }
            ";

            using var assembly = Compile(cs);

            // ACT
            var sut = SingleCommandApplicationDocumentation.FromAssemblyDefinition(assembly, NullLogger.Instance);

            // ASSERT
            Assert.DoesNotContain(sut.Command.Options, c => c.Name == "option3");
        }

        [Fact]
        public void Expected_values_exist()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;

                public class Class1
                {
                    [Value(0)]
                    public string Value1 { get; set; }

                    [Value(1)]
                    public string Value2 { get; set; }
                }
            ";

            using var assembly = Compile(cs);

            // ACT
            var sut = SingleCommandApplicationDocumentation.FromAssemblyDefinition(assembly, NullLogger.Instance);

            // ASSERT
            Assert.Equal(2, sut.Command.Values.Count);
            Assert.Contains(sut.Command.Values, c => c.Index == 0);
            Assert.Contains(sut.Command.Values, c => c.Index == 1);
        }
    }
}
