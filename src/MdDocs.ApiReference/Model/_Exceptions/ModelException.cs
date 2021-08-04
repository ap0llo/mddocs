using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public abstract class ModelException : Exception
    {
        public ModelException(string? message) : base(message)
        { }
    }
}
