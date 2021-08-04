using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    // TODO 2021-08-04: Remove once type is no longer used
    internal class InvalidAssemblySetException : Exception
    {
        public InvalidAssemblySetException(string? message) : base(message)
        {
        }
    }
}
