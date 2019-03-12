using System;

namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    public class MemberIdParserException : Exception
    {
        public MemberIdParserException()
        { }

        public MemberIdParserException(string message) : base(message)
        { }

        public MemberIdParserException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
