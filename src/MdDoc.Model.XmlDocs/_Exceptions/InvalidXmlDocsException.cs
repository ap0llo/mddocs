using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Model.XmlDocs
{
    public class InvalidXmlDocsException : Exception
    {
        public InvalidXmlDocsException(string message) : base(message)
        {
        }

        public InvalidXmlDocsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
