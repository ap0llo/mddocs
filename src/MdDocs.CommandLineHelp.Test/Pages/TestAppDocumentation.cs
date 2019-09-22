using System;
using Grynwald.MdDocs.CommandLineHelp.Model;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Pages
{
    internal class TestAppDocumentation : ApplicationDocumentation
    {
        public TestAppDocumentation() : base("TestApp", "1.2.3", Array.Empty<string>())
        {
        }
    }
}
