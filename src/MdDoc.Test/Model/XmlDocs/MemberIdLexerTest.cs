using MdDoc.Model.XmlDocs;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace MdDoc.Test.Model.XmlDocs
{
    public class MemberIdLexerTest
    {
        public class MemberIdLexerTestCase : IXunitSerializable
        {
            public string Input { get; private set; }

            internal IReadOnlyList<Token> ExpectedTokens { get; private set; }


            // parameterless constructor required by xunit
            public MemberIdLexerTestCase()
            { }

            internal MemberIdLexerTestCase(string input, params Token[] expectedTokens)
            {
                Input = input;
                ExpectedTokens = expectedTokens;
            }


            public void Deserialize(IXunitSerializationInfo info)
            {
                Input = info.GetValue<string>(nameof(Input));

                var tokenCount = info.GetValue<int>("tokenCount");
                var tokens = new List<Token>(tokenCount);
                for (int i = 0; i < tokenCount; i++)
                {

                    var kind = Enum.Parse<TokenKind>(info.GetValue<string>($"tokenKind{i}"));
                    var value = info.GetValue<string>($"tokenValue{i}");
                    tokens.Add(new Token(kind, value));
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
                {
                    yield return new object[] { testCase };
                }

            }

            public IEnumerable<MemberIdLexerTestCase> GetTestCases()
            {
                yield return new MemberIdLexerTestCase("", new Token(TokenKind.Eof, ""));
                yield return new MemberIdLexerTestCase("T:", new Token(TokenKind.IdentifierType, "T"), new Token(TokenKind.Colon, ":"), new Token(TokenKind.Eof, ""));
                yield return new MemberIdLexerTestCase("F:", new Token(TokenKind.IdentifierType, "F"), new Token(TokenKind.Colon, ":"), new Token(TokenKind.Eof, ""));
                yield return new MemberIdLexerTestCase("P:", new Token(TokenKind.IdentifierType, "P"), new Token(TokenKind.Colon, ":"), new Token(TokenKind.Eof, ""));
                yield return new MemberIdLexerTestCase("M:", new Token(TokenKind.IdentifierType, "M"), new Token(TokenKind.Colon, ":"), new Token(TokenKind.Eof, ""));
                yield return new MemberIdLexerTestCase("E:", new Token(TokenKind.IdentifierType, "E"), new Token(TokenKind.Colon, ":"), new Token(TokenKind.Eof, ""));
                yield return new MemberIdLexerTestCase(
                    "T:Namespace",
                    new Token(TokenKind.IdentifierType, "T"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Name, "Namespace"),
                    new Token(TokenKind.Eof, "")
                );
                yield return new MemberIdLexerTestCase(
                    "T:Namespace1.Namespace2",
                    new Token(TokenKind.IdentifierType, "T"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Name, "Namespace1"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Namespace2"),
                    new Token(TokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "F:System.String.Length",
                    new Token(TokenKind.IdentifierType, "F"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Name, "System"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "String"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Length"),
                    new Token(TokenKind.Eof, "")
                );
                yield return new MemberIdLexerTestCase(
                    "F:System.String`2.Length",
                    new Token(TokenKind.IdentifierType, "F"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Name, "System"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "String"),
                    new Token(TokenKind.Backtick, "`"),
                    new Token(TokenKind.Number, "2"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Length"),
                    new Token(TokenKind.Eof, "")
                );
                yield return new MemberIdLexerTestCase(
                    "M:String.#ctor",
                    new Token(TokenKind.IdentifierType, "M"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Name, "String"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, ".ctor"),
                    new Token(TokenKind.Eof, "")
                );
                yield return new MemberIdLexerTestCase(
                    "T:GenericType`2",
                    new Token(TokenKind.IdentifierType, "T"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Name, "GenericType"),
                    new Token(TokenKind.Backtick, "`"),
                    new Token(TokenKind.Number, "2"),
                    new Token(TokenKind.Eof, "")
                );
                yield return new MemberIdLexerTestCase(
                    "M:Namespace.Type.Method(System.String)",
                    new Token(TokenKind.IdentifierType, "M"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Name, "Namespace"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Type"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Method"),
                    new Token(TokenKind.OpenParenthesis, "("),
                    new Token(TokenKind.Name, "System"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "String"),
                    new Token(TokenKind.CloseParenthesis, ")"),
                    new Token(TokenKind.Eof, "")
                );
                yield return new MemberIdLexerTestCase(
                    "M:Namespace.Type.Method(System.String,System.Int32)",
                    new Token(TokenKind.IdentifierType, "M"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Name, "Namespace"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Type"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Method"),
                    new Token(TokenKind.OpenParenthesis, "("),
                    new Token(TokenKind.Name, "System"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "String"),
                    new Token(TokenKind.Comma, ","),
                    new Token(TokenKind.Name, "System"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Int32"),
                    new Token(TokenKind.CloseParenthesis, ")"),
                    new Token(TokenKind.Eof, "")
                );
                yield return new MemberIdLexerTestCase(
                    "M:Namespace.Type.Method``1(``0,System.Int32)",
                    new Token(TokenKind.IdentifierType, "M"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Name, "Namespace"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Type"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Method"),
                    new Token(TokenKind.DoubleBacktick, "``"),
                    new Token(TokenKind.Number, "1"),                    
                    new Token(TokenKind.OpenParenthesis, "("),
                    new Token(TokenKind.DoubleBacktick, "``"),
                    new Token(TokenKind.Number, "0"),
                    new Token(TokenKind.Comma, ","),
                    new Token(TokenKind.Name, "System"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Int32"),
                    new Token(TokenKind.CloseParenthesis, ")"),
                    new Token(TokenKind.Eof, "")
                );
                yield return new MemberIdLexerTestCase(
                    "M:Namespace.Type.Method(Type2{System.String})",
                    new Token(TokenKind.IdentifierType, "M"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Name, "Namespace"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Type"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Method"),
                    new Token(TokenKind.OpenParenthesis, "("),
                    new Token(TokenKind.Name, "Type2"),
                    new Token(TokenKind.OpenBrace, "{"),
                    new Token(TokenKind.Name, "System"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "String"),
                    new Token(TokenKind.CloseBrace, "}"),
                    new Token(TokenKind.CloseParenthesis, ")"),
                    new Token(TokenKind.Eof, "")
                );
                yield return new MemberIdLexerTestCase(
                    "M:Namespace.Type.op_Implicit(Type)~System.String",
                    new Token(TokenKind.IdentifierType, "M"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Name, "Namespace"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Type"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "op_Implicit"),
                    new Token(TokenKind.OpenParenthesis, "("),
                    new Token(TokenKind.Name, "Type"),
                    new Token(TokenKind.CloseParenthesis, ")"),
                    new Token(TokenKind.Tilde, "~"),
                    new Token(TokenKind.Name, "System"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "String"),
                    new Token(TokenKind.Eof, "")
                );
                yield return new MemberIdLexerTestCase(
                    "M:Namespace.Type.Method(System.Int32[])",
                    new Token(TokenKind.IdentifierType, "M"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Name, "Namespace"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Type"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Method"),
                    new Token(TokenKind.OpenParenthesis, "("),
                    new Token(TokenKind.Name, "System"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Int32"),
                    new Token(TokenKind.OpenSquareBracket, "["),
                    new Token(TokenKind.CloseSquareBracket, "]"),
                    new Token(TokenKind.CloseParenthesis, ")"),
                    new Token(TokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "M:Type.Method(object[,])",
                    new Token(TokenKind.IdentifierType, "M"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Name, "Type"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Method"),
                    new Token(TokenKind.OpenParenthesis, "("),
                    new Token(TokenKind.Name, "object"),
                    new Token(TokenKind.OpenSquareBracket, "["),
                    new Token(TokenKind.Comma, ","),
                    new Token(TokenKind.CloseSquareBracket, "]"),
                    new Token(TokenKind.CloseParenthesis, ")"),
                    new Token(TokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "M:Type.Method(object[1:,])",
                    new Token(TokenKind.IdentifierType, "M"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Name, "Type"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Method"),
                    new Token(TokenKind.OpenParenthesis, "("),
                    new Token(TokenKind.Name, "object"),
                    new Token(TokenKind.OpenSquareBracket, "["),
                    new Token(TokenKind.Number, "1"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Comma, ","),
                    new Token(TokenKind.CloseSquareBracket, "]"),
                    new Token(TokenKind.CloseParenthesis, ")"),
                    new Token(TokenKind.Eof, "")
                );

                yield return new MemberIdLexerTestCase(
                    "M:Type.Method(object[1:2,])",
                    new Token(TokenKind.IdentifierType, "M"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Name, "Type"),
                    new Token(TokenKind.Dot, "."),
                    new Token(TokenKind.Name, "Method"),
                    new Token(TokenKind.OpenParenthesis, "("),
                    new Token(TokenKind.Name, "object"),
                    new Token(TokenKind.OpenSquareBracket, "["),
                    new Token(TokenKind.Number, "1"),
                    new Token(TokenKind.Colon, ":"),
                    new Token(TokenKind.Number, "2"),
                    new Token(TokenKind.Comma, ","),
                    new Token(TokenKind.CloseSquareBracket, "]"),
                    new Token(TokenKind.CloseParenthesis, ")"),
                    new Token(TokenKind.Eof, "")
                );
            }
        }


        [Theory]
        [MemberIdLexerTestData]
        public void Tokenizer_returns_expected_Tokens(MemberIdLexerTestCase testCase)
        {
            var tokenizer = new MemberIdLexer(testCase.Input);
            var actualTokens = tokenizer.GetTokens();

            Assert.NotNull(actualTokens);
            Assert.Equal(testCase.ExpectedTokens.Count, actualTokens.Count);
            for (int i = 0; i < testCase.ExpectedTokens.Count; i++)
            {
                Assert.Equal(testCase.ExpectedTokens[i].Kind, actualTokens[i].Kind);
                Assert.Equal(testCase.ExpectedTokens[i].Value, actualTokens[i].Value);
            }
        }        
    }
}
