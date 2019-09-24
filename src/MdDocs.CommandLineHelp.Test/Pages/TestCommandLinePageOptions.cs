using System;
using System.Collections.Generic;
using System.Text;
using Grynwald.MdDocs.CommandLineHelp.Pages;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Pages
{
    class TestCommandLinePageOptions : ICommandLinePageOptions
    {
        public bool IncludeVersion { get; set; } = true;
    }
}
