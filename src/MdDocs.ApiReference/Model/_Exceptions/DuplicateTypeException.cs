using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    internal sealed class DuplicateTypeException : Exception
    {
        public string TypeName { get; }


        public DuplicateTypeException(string typeName, string message)
        {
            TypeName = typeName;
        }
    }
}
