namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    internal enum MemberIdTokenKind
    {
        /// <summary>
        /// A token that indicates the type of identifier (N, T, M, P, F, E)
        /// </summary>
        IdentifierType,
        /// <summary>
        /// A colon (':') token
        /// </summary>
        Colon,
        /// <summary>
        /// A dot ('.') token
        /// </summary>
        Dot,
        /// <summary>
        /// A name token (either a namespace, type or method name)
        /// </summary>
        Name,
        /// <summary>
        /// A backtick ('`') token
        /// </summary>
        Backtick,
        /// <summary>
        /// A double-backtick ('``') token
        /// </summary>
        DoubleBacktick,
        /// <summary>
        /// A number token
        /// </summary>
        Number,
        /// <summary>
        /// A opening parenthesis ('(') token
        /// </summary>
        OpenParenthesis,
        /// <summary>
        /// A closing parenthesis (')') token
        /// </summary>
        CloseParenthesis,
        /// <summary>
        /// A comma (',') token
        /// </summary>
        Comma,
        /// <summary>
        /// A opening brace ('{') token
        /// </summary>
        OpenBrace,
        /// <summary>
        /// A closing brace ('}') token
        /// </summary>
        CloseBrace,
        /// <summary>
        /// A tile ('~') token
        /// </summary>
        Tilde,
        /// <summary>
        /// A open square bracket ('[') token
        /// </summary>
        OpenSquareBracket,
        /// <summary>
        /// A close square bracket (']') token
        /// </summary>
        CloseSquareBracket,
        /// <summary>
        /// A at('@') token
        /// </summary>
        At,
        /// <summary>
        /// A token indicating the end of the text to parse
        /// </summary>
        Eof
    }
}
