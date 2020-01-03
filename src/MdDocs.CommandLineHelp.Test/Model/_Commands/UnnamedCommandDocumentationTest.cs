using System;
using System.Linq;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    public class UnnamedCommandDocumentationTest : CommandLineDynamicCompilationTestBase
    {
        private UnnamedCommandDocumentation GetUnnamedCommandDocumentation(TypeDefinition definition)
        {
            return UnnamedCommandDocumentation.FromTypeDefinition(new MultiCommandApplicationDocumentation("Test"), definition, NullLogger.Instance);
        }

        [Fact]
        public void FromTypeDefinition_throws_ArgumentException_for_type_definitions_with_Verb_attributes()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;
               
                [Verb(""command"")]
                public class CommandOptions
                {
                }
            ";

            using var assembly = Compile(cs);

            var definition = assembly.MainModule.Types.Single(x => x.Name == "CommandOptions");

            var application = new MultiCommandApplicationDocumentation("Test");

            // ACT / ASSERT
            Assert.Throws<ArgumentException>(() => UnnamedCommandDocumentation.FromTypeDefinition(application, definition, NullLogger.Instance));
        }

        [Fact]
        public void Expected_option_name_exists()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;
                               
                public class Options
                {
                    [Option(""option1"")]
                    public string Option1 { get; set; }

                    [Option(""option2"")]
                    public int Option2 { get; set; }
                }
            ";

            using var assembly = Compile(cs);

            // ACT
            var command = GetUnnamedCommandDocumentation(assembly.MainModule.Types.Single(x => x.Name == "Options"));

            // ASSERT
            Assert.Contains(command.Options, o => o.Name == "option1");
            Assert.Contains(command.Options, o => o.Name == "option2");
        }

        [Fact]
        public void Hidden_options_are_ignored()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;
                               
                public class Options
                {
                    [Option(""option1"")]
                    public string Option1 { get; set; }

                    [Option(""option2"", Hidden = true)]
                    public int Option2 { get; set; }
                }
            ";

            using var assembly = Compile(cs);

            // ACT
            var command = GetUnnamedCommandDocumentation(assembly.MainModule.Types.Single(x => x.Name == "Options"));

            // ASSERT            
            Assert.DoesNotContain(command.Options, o => o.Name == "option2");
        }

        [Fact]
        public void Expected_option_short_name_exists()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;
                               
                public class Options
                {
                    [Option('x', ""option1"")]
                    public string Option1 { get; set; }

                    [Option('y', ""option2"")]
                    public int Option2 { get; set; }
                }
            ";

            using var assembly = Compile(cs);

            // ACT
            var command = GetUnnamedCommandDocumentation(assembly.MainModule.Types.Single(x => x.Name == "Options"));

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
                               
                public class Options
                {
                     [Value(0)]
                    public string Value1 { get; set; }

                    [Value(1, MetaName = ""Value2 name"")]
                    public string Value2 { get; set; }
                }
            ";

            using var assembly = Compile(cs);

            // ACT
            var command = GetUnnamedCommandDocumentation(assembly.MainModule.Types.Single(x => x.Name == "Options"));

            // ASSERT
            Assert.Contains(command.Values, o => o.Index == 0);
            Assert.Contains(command.Values, o => o.Index == 1);
        }
    }
}
