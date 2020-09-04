using System;

namespace Grynwald.MdDocs.MSBuild.IntegrationTest
{
    public class VSWhereException : Exception
    {
        public VSWhereException(string message) : base(message)
        { }
    }
}
