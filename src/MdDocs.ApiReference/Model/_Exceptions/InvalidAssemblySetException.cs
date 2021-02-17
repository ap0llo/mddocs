using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    internal class InvalidAssemblySetException : Exception
    {
        public InvalidAssemblySetException(string? message) : base(message)
        {
        }
    }
}
