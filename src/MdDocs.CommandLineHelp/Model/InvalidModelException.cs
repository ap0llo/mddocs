using System;

namespace Grynwald.MdDocs.CommandLineHelp.Model
{
    public class InvalidModelException : Exception
    {
        public InvalidModelException(string message) : base(message)
        { }
    }
}
