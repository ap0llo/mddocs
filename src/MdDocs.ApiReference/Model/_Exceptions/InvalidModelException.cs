using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public abstract class InvalidModelException : Exception
    {
        public InvalidModelException(string? message) : base(message)
        { }
    }
}
