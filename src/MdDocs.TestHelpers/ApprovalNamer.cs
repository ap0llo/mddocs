using System.IO;
using ApprovalTests.Namers;

namespace Grynwald.MdDocs.TestHelpers
{
    public class ApprovalNamer : UnitTestFrameworkNamer
    {
        private readonly string? m_TypeName;

        public override string Subdirectory => Path.Combine(base.Subdirectory, "_referenceResults");

        public override string Name
        {
            get
            {
                var baseName = base.Name; ;
                if (m_TypeName == null)
                {
                    return baseName;
                }
                else
                {
                    // base name uses <TYPENAME>.<METHODNAME> format
                    // to make the namer work with test cases implemented in base classes
                    // replace the type name with the name passed to the constructor
                    return $"{m_TypeName}.{baseName.Substring(baseName.IndexOf(".") + 1)}";
                }
            }

        }


        public ApprovalNamer(string? typeName = null)
        {
            m_TypeName = typeName;
        }
    }
}
