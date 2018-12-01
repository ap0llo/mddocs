using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Test.TestData
{
    /// <summary>
    /// This is a test class for testing parsing of XML documentation
    /// </summary>
    public class TestClass_XmlDocs
    {
        /// <summary>
        /// Line1
        /// </summary>
        public void TestMethod_Summary_01() { }

        /// <summary>
        /// Line1
        /// Line2
        /// </summary>
        public void TestMethod_Summary_02() { }

        /// <summary>
        /// Line1
        /// 
        /// Line2
        /// </summary>
        public void TestMethod_Summary_03() { }
    }
}
