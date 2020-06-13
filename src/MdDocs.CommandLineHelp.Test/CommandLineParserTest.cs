using System;
using System.Collections.Generic;
using CommandLine;
using Xunit;
using Xunit.Sdk;

namespace Grynwald.MdDocs.CommandLineHelp.Test
{
    //TODO: Move to Loaders namespace
    /// <summary>
    /// Test class that verifies assumptions about the CommandLineParser library MdDocs makes are true
    /// </summary>
    public class CommandLineParserTest
    {
        private class Options1
        {
            [Option('x', "parameter1")]
            public bool Parameter1 { get; set; }
        }

        private class Options2
        {
            [Option('x', "parameter1", Required = true)]
            public bool Parameter1 { get; set; }
        }

        [Theory]
        [InlineData(new[] { "--parameter1" }, true)]
        [InlineData(new[] { "-x" }, true)]
        [InlineData(new string[0], false)]
        // value after parameter is ignored: 
        [InlineData(new[] { "--parameter1", "false" }, true)]
        [InlineData(new[] { "--parameter1", "true" }, true)]
        [InlineData(new[] { "-x", "false" }, true)]
        [InlineData(new[] { "-x", "true" }, true)]
        public void Boolean_options_are_treated_as_switch_parameters(string[] args, bool expected)
        {
            var parsed = Parse<Options1>(args);
            Assert.Equal(expected, parsed.Parameter1);
        }

        [Theory]
        [InlineData(new[] { "--parameter1" }, true)]
        [InlineData(new[] { "-x" }, true)]
        public void Boolean_options_do_not_require_a_value(string[] args, bool expected)
        {
            var parsed = Parse<Options2>(args);
            Assert.Equal(expected, parsed.Parameter1);
        }

        private class Options3
        {
            [Option("parameter1")]
            public bool? Parameter1 { get; set; }
        }

        [Theory]
        [InlineData(new string[0], null)]
        [InlineData(new[] { "--parameter1", "true" }, true)]
        [InlineData(new[] { "--parameter1", "false" }, false)]
        public void Nullable_boolean_options_are_not_treated_as_switch_parameters(string[] args, bool? expected)
        {
            // ARRANGE / ACT
            var parsed = Parse<Options3>(args);

            // ASSERT
            Assert.Equal(expected, parsed.Parameter1);
        }


        private class Options4
        {
            [Value(0, Default = "my-default")]
            public string? Parameter1 { get; set; }
        }

        [Fact]
        public void Values_can_have_default_values()
        {
            // ARRANGE / ACT
            var parsed = Parse<Options4>(Array.Empty<string>());

            // ASSERT
            Assert.Equal("my-default", parsed.Parameter1);
        }


        private T Parse<T>(string[] args)
        {
            return Parser.Default
             .ParseArguments<T>(args)
             .MapResult(
                 (T opts) => opts,
                 (IEnumerable<Error> errors) => throw new XunitException($"CommandLine parsing failed: {String.Join(", ", errors)} ")
             );
        }
    }
}
