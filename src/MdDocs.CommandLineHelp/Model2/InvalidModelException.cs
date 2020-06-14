using System;
using System.Collections.Generic;
using System.Text;

namespace Grynwald.MdDocs.CommandLineHelp.Model2
{
    public class InvalidModelException : Exception
    {
        public InvalidModelException(string message) : base(message)
        { }
    }
}
