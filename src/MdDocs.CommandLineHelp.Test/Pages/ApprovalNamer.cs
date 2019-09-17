using System.IO;
using ApprovalTests.Namers;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Pages
{
    internal class ApprovalNamer : UnitTestFrameworkNamer
    {
        public override string Subdirectory => Path.Combine(base.Subdirectory, "_referenceResults");
    }
}
