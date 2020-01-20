using System.Linq;
using Grynwald.MdDocs.CommandLineHelp.Model;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    public class MultiCommandApplicationDocumentationTest : CommandLineDynamicCompilationTestBase
    {
        [Fact]
        public void Commands_returns_expected_number_of_commands()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;

                [Verb(""command1"")]
                public class Command1Options
                { }

                [Verb(""command2"")]
                public class Command2Options
                { }
            ";

            using var assembly = Compile(cs);

            // ACT
            var sut = MultiCommandApplicationDocumentation.FromAssemblyDefinition(assembly, NullLogger.Instance);

            // ASSERT
            Assert.Equal(2, sut.Commands.Count);
        }

        [Fact]
        public void Abstract_types_are_ignored()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;

                [Verb(""command1"")]
                public class Command1Options
                { }

                [Verb(""command2"")]
                public class Command2Options
                { }

                [Verb(""command3"")]
                public abstract class Command3Options
                { }
            ";

            using var assembly = Compile(cs);

            // ACT
            var sut = MultiCommandApplicationDocumentation.FromAssemblyDefinition(assembly, NullLogger.Instance);

            // ASSERT
            Assert.DoesNotContain(sut.Commands, c => c.Name == "command3");
        }

        [Fact]
        public void Expected_command_exists()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;

                [Verb(""command1"")]
                public class Command1Options
                { }

                [Verb(""command2"")]
                public class Command2Options
                { }

                [Verb(""command3"")]
                public class Command3Options
                { }
            ";

            using var assembly = Compile(cs);

            // ACT
            var sut = MultiCommandApplicationDocumentation.FromAssemblyDefinition(assembly, NullLogger.Instance);

            // ASSERT
            Assert.Equal(3, sut.Commands.Count);
            Assert.Contains(sut.Commands, c => c.Name == "command1");
            Assert.Contains(sut.Commands, c => c.Name == "command2");
            Assert.Contains(sut.Commands, c => c.Name == "command3");
        }

        [Fact]
        public void Name_returns_expected_value()
        {
            // ARRANGE
            var cs = @"
                using System.Reflection;
                using CommandLine;

                [assembly: AssemblyTitle(""TestDataAssemblyTitle"")]

                [Verb(""command1"")]
                public class Command1Options
                { }
            ";

            using var assembly = Compile(cs);

            // ACT
            var sut = MultiCommandApplicationDocumentation.FromAssemblyDefinition(assembly, NullLogger.Instance);

            // ASSERT
            Assert.Equal("TestDataAssemblyTitle", sut.Name);
        }

        [Fact]
        public void Usage_returns_expected_values()
        {
            // ARRANGE
            var cs = @"
                using System.Reflection;
                using CommandLine;
                using CommandLine.Text;

                [assembly: AssemblyUsage(""AssemblyUsage Line 1"", ""AssemblyUsage Line 2"")]

                [Verb(""command1"")]
                public class Command1Options
                { }
            ";

            using var assembly = Compile(cs);

            // ACT
            var sut = MultiCommandApplicationDocumentation.FromAssemblyDefinition(assembly, NullLogger.Instance);

            // ASSERT
            Assert.NotNull(sut.Usage);
            Assert.Equal(2, sut.Usage.Count);
            Assert.Equal("AssemblyUsage Line 1", sut.Usage.First());
            Assert.Equal("AssemblyUsage Line 2", sut.Usage.Last());
        }

        [Fact]
        public void Usage_is_empty_for_assemblies_without_usage_attribute()
        {
            // ARRANGE
            var cs = @"
                using System.Reflection;
                using CommandLine;

                [Verb(""command1"")]
                public class Command1Options
                { }
            ";

            using var assembly = Compile(cs);

            // ACT
            var sut = MultiCommandApplicationDocumentation.FromAssemblyDefinition(assembly, NullLogger.Instance);

            // ASSERT
            Assert.NotNull(sut.Usage);
            Assert.Empty(sut.Usage);
        }

        [Fact]
        public void Hidden_option_classes_are_ignored()
        {
            // ARRANGE
            var cs = @"
                using CommandLine;

                [Verb(""command1"")]
                public class Command1Options
                { }

                [Verb(""command2"", Hidden = true)]
                public class Command2Options
                { }
            ";

            using var assembly = Compile(cs);

            // ACT
            var sut = MultiCommandApplicationDocumentation.FromAssemblyDefinition(assembly, NullLogger.Instance);

            // ASSERT
            var command = Assert.Single(sut.Commands);
            Assert.Equal("command1", command.Name);
        }
    }
}
