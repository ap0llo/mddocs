using System;
using System.Collections.Generic;
using System.Text;

namespace Grynwald.MdDocs.Common.Templates
{
    [Serializable]
    public class InvalidTemplateConfigurationException : Exception
    {
        public InvalidTemplateConfigurationException(string message) : base(message)
        { }
    }
}
