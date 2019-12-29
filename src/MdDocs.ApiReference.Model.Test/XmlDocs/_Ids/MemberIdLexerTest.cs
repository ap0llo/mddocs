using System;
using System.Collections.Generic;
using System.Reflection;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Grynwald.MdDocs.ApiReference.Test.Model.XmlDocs
{
    public class MemberIdLexerTest
    {
        public class MemberIdLexerTestCase : IXunitSerializable
        {
            public string Input { get; private set; }

            internal IReadOnlyList<MemberIdToken> ExpectedTokens { get; private set; }


            // parameterless constructor required by xunit
            public MemberIdLexerTestCase()
            {
                Input = null!;          // set by Serialize()
                ExpectedTokens = null!; // set by Serialize()
            }

            internal MemberIdLexerTestCase(string input, params MemberIdToken[] expectedTokens)
            {
                Input = input;
                ExpectedTokens = expectedTokens;
            }


            public void Deserialize(IXunitSerializationInfo info)
            {
                Input = info.GetValue<string>(nameof(Input));

                var tokenCount = info.GetValue<int>("tokenCount");
                var tokens = new List<MemberIdToken>(tokenCount);
                for (int i = 0; i < tokenCount; i++)
                {

                    var kind = Enum.Parse<MemberIdTokenKind>(info.GetValue<string>($"tokenKind{i}"));
                    var value = info.GetValue<string>($"tokenValue{i}");
                    tokens.Add(new MemberIdToken(kind, value));
                }

                ExpectedTokens = tokens;
            }

            public void Serialize(IXunitSerializationInfo info)
            {
                info.AddValue(nameof(Input), Input);
                info.AddValue("tokenCount", ExpectedTokens.Count);
                for (int i = 0; i < ExpectedTokens.Count; i++)
                {
                    info.AddValue($"tokenKind{i}", ExpectedTokens[i].Kind.ToString());
                    info.AddValue($"tokenValue{i}", ExpectedTokens[i].Value);
                }
            }

            public override string ToString() => Input;
        }

        public class MemberIdLexerTestDataAttribute : DataAttribute
        {
            public override IEnumerable<object[]> GetData(MethodInfo testMethod)
            {
                foreach (var testCase in GetTestCases())
                    yield return new object[] { testCase };
            }

            public IEnumerable<MemberIdLexerTestCase> GetTestCases()
            {
                yield return new MemberIdLexerTestCase("", new MemberIdToken(MemberIdTokenKind.Eof, ""));
                yield return new MemberIdLexerTestCase("N:", new MemberIdToken(MemberIdTokenKind.IdentifierType, "N"), new MemberIdToken(MemberIdTokenKind.Colon, ":"), new MemberIdToken(MemberIdTokenKind.Eof, ""));
                yield return new MemberIdLexerTestCase("T:", new MemberIdToken(MemberIdTokenKind.IdentifierType, "T"), new MemberIdToken(MemberIdTokenKind.Colon, ":"), new MemberIdToken(MemberIdTokenKind.Eof, ""));
                yield return new MemberIdLexerTestCase("F:", new MemberIdToken(MemberIdTokenKind.IdentifierType, "F"), new MemberIdToken(MemberIdTokenKind.Colon, ":"), new MemberIdToken(MemberIdTokenKind.Eof, ""));
                yield return new MemberIdLexerTestCase("P:", new MemberIdToken(MemberIdTokenKind.IdentifierType, "P"), new MemberIdToken(MemberIdTokenKind.Colon, ":"), new MemberIdToken(MemberIdTokenKind.Eof, ""));
                yield return new MemberIdLexerTestCase("M:", new MemberIdToken(MemberIdTokenKind.IdentifierType, "M"), new MemberIdToken(MemberIdTokenKind.Colon, ":"), new MemberIdToken(MemberIdTokenKind.Eof, ""));
                yield return new MemberIdLexerTestCase("E:", new MemberIdToken(MemberIdTokenKind.IdentifierType, "E"), new MemberIdToken(MemberIdTokenKind.Colon, ":"), new MemberIdToken(MemberIdTokenKind.Eof, ""));

                yield return new MemberIdLexerTestCase(
                    "N:System",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "N"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "System"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "N:System.Collections",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "N"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "System"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Collections"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "T:Namespace",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "T"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "Namespace"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "T:Namespace1.Namespace2",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "T"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "Namespace1"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Namespace2"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "F:System.String.Length",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "F"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "System"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "String"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Length"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "F:System.String`2.Length",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "F"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "System"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "String"),
                    new MemberIdToken(MemberIdTokenKind.Backtick, "`"),
                    new MemberIdToken(MemberIdTokenKind.Number, "2"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Length"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "M:String.#ctor",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "M"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "String"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, ".ctor"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "T:GenericType`2",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "T"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "GenericType"),
                    new MemberIdToken(MemberIdTokenKind.Backtick, "`"),
                    new MemberIdToken(MemberIdTokenKind.Number, "2"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "M:Namespace.Type.Method(System.String)",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "M"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "Namespace"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Type"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Method"),
                    new MemberIdToken(MemberIdTokenKind.OpenParenthesis, "("),
                    new MemberIdToken(MemberIdTokenKind.Name, "System"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "String"),
                    new MemberIdToken(MemberIdTokenKind.CloseParenthesis, ")"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "M:Namespace.Type.Method(System.String,System.Int32)",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "M"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "Namespace"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Type"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Method"),
                    new MemberIdToken(MemberIdTokenKind.OpenParenthesis, "("),
                    new MemberIdToken(MemberIdTokenKind.Name, "System"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "String"),
                    new MemberIdToken(MemberIdTokenKind.Comma, ","),
                    new MemberIdToken(MemberIdTokenKind.Name, "System"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Int32"),
                    new MemberIdToken(MemberIdTokenKind.CloseParenthesis, ")"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "M:Namespace.Type.Method``1(``0,System.Int32)",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "M"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "Namespace"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Type"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Method"),
                    new MemberIdToken(MemberIdTokenKind.DoubleBacktick, "``"),
                    new MemberIdToken(MemberIdTokenKind.Number, "1"),
                    new MemberIdToken(MemberIdTokenKind.OpenParenthesis, "("),
                    new MemberIdToken(MemberIdTokenKind.DoubleBacktick, "``"),
                    new MemberIdToken(MemberIdTokenKind.Number, "0"),
                    new MemberIdToken(MemberIdTokenKind.Comma, ","),
                    new MemberIdToken(MemberIdTokenKind.Name, "System"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Int32"),
                    new MemberIdToken(MemberIdTokenKind.CloseParenthesis, ")"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "M:Namespace.Type.Method(Type2{System.String})",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "M"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "Namespace"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Type"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Method"),
                    new MemberIdToken(MemberIdTokenKind.OpenParenthesis, "("),
                    new MemberIdToken(MemberIdTokenKind.Name, "Type2"),
                    new MemberIdToken(MemberIdTokenKind.OpenBrace, "{"),
                    new MemberIdToken(MemberIdTokenKind.Name, "System"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "String"),
                    new MemberIdToken(MemberIdTokenKind.CloseBrace, "}"),
                    new MemberIdToken(MemberIdTokenKind.CloseParenthesis, ")"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "M:Namespace.Type.op_Implicit(Type)~System.String",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "M"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "Namespace"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Type"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "op_Implicit"),
                    new MemberIdToken(MemberIdTokenKind.OpenParenthesis, "("),
                    new MemberIdToken(MemberIdTokenKind.Name, "Type"),
                    new MemberIdToken(MemberIdTokenKind.CloseParenthesis, ")"),
                    new MemberIdToken(MemberIdTokenKind.Tilde, "~"),
                    new MemberIdToken(MemberIdTokenKind.Name, "System"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "String"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "M:Namespace.Type.Method(System.Int32[])",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "M"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "Namespace"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Type"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Method"),
                    new MemberIdToken(MemberIdTokenKind.OpenParenthesis, "("),
                    new MemberIdToken(MemberIdTokenKind.Name, "System"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Int32"),
                    new MemberIdToken(MemberIdTokenKind.OpenSquareBracket, "["),
                    new MemberIdToken(MemberIdTokenKind.CloseSquareBracket, "]"),
                    new MemberIdToken(MemberIdTokenKind.CloseParenthesis, ")"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "M:Type.Method(object[,])",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "M"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "Type"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Method"),
                    new MemberIdToken(MemberIdTokenKind.OpenParenthesis, "("),
                    new MemberIdToken(MemberIdTokenKind.Name, "object"),
                    new MemberIdToken(MemberIdTokenKind.OpenSquareBracket, "["),
                    new MemberIdToken(MemberIdTokenKind.Comma, ","),
                    new MemberIdToken(MemberIdTokenKind.CloseSquareBracket, "]"),
                    new MemberIdToken(MemberIdTokenKind.CloseParenthesis, ")"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "M:Type.Method(object[1:,])",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "M"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "Type"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Method"),
                    new MemberIdToken(MemberIdTokenKind.OpenParenthesis, "("),
                    new MemberIdToken(MemberIdTokenKind.Name, "object"),
                    new MemberIdToken(MemberIdTokenKind.OpenSquareBracket, "["),
                    new MemberIdToken(MemberIdTokenKind.Number, "1"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Comma, ","),
                    new MemberIdToken(MemberIdTokenKind.CloseSquareBracket, "]"),
                    new MemberIdToken(MemberIdTokenKind.CloseParenthesis, ")"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "M:Type.Method(object[1:2,])",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "M"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "Type"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Method"),
                    new MemberIdToken(MemberIdTokenKind.OpenParenthesis, "("),
                    new MemberIdToken(MemberIdTokenKind.Name, "object"),
                    new MemberIdToken(MemberIdTokenKind.OpenSquareBracket, "["),
                    new MemberIdToken(MemberIdTokenKind.Number, "1"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Number, "2"),
                    new MemberIdToken(MemberIdTokenKind.Comma, ","),
                    new MemberIdToken(MemberIdTokenKind.CloseSquareBracket, "]"),
                    new MemberIdToken(MemberIdTokenKind.CloseParenthesis, ")"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "M:Type.Method(System.String@)",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "M"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "Type"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Method"),
                    new MemberIdToken(MemberIdTokenKind.OpenParenthesis, "("),
                    new MemberIdToken(MemberIdTokenKind.Name, "System"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "String"),
                    new MemberIdToken(MemberIdTokenKind.At, "@"),
                    new MemberIdToken(MemberIdTokenKind.CloseParenthesis, ")"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "M:Type.Method(System.String[]@)",
                    new MemberIdToken(MemberIdTokenKind.IdentifierType, "M"),
                    new MemberIdToken(MemberIdTokenKind.Colon, ":"),
                    new MemberIdToken(MemberIdTokenKind.Name, "Type"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "Method"),
                    new MemberIdToken(MemberIdTokenKind.OpenParenthesis, "("),
                    new MemberIdToken(MemberIdTokenKind.Name, "System"),
                    new MemberIdToken(MemberIdTokenKind.Dot, "."),
                    new MemberIdToken(MemberIdTokenKind.Name, "String"),
                    new MemberIdToken(MemberIdTokenKind.OpenSquareBracket, "["),
                    new MemberIdToken(MemberIdTokenKind.CloseSquareBracket, "]"),
                    new MemberIdToken(MemberIdTokenKind.At, "@"),
                    new MemberIdToken(MemberIdTokenKind.CloseParenthesis, ")"),
                    new MemberIdToken(MemberIdTokenKind.Eof, "")
                );
            }
        }


        [Theory]
        [MemberIdLexerTestData]
        public void Tokenizer_returns_expected_Tokens(MemberIdLexerTestCase testCase)
        {
            // ARRANGE
            var tokenizer = new MemberIdLexer(testCase.Input);

            // ACT
            var actualTokens = tokenizer.GetTokens();

            // ASSERT
            Assert.NotNull(actualTokens);
            Assert.Equal(testCase.ExpectedTokens.Count, actualTokens.Count);
            for (var i = 0; i < testCase.ExpectedTokens.Count; i++)
            {
                Assert.Equal(testCase.ExpectedTokens[i].Kind, actualTokens[i].Kind);
                Assert.Equal(testCase.ExpectedTokens[i].Value, actualTokens[i].Value);
            }
        }
    }
}
