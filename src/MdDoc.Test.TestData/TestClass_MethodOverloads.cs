using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Test.TestData
{
    /// <summary>
    /// Test class for method overloads
    /// </summary>
    public class TestClass_MethodOverloads
    {
        /// <summary>
        /// Test Method
        /// </summary>
        public void TestMethod1() { }

        /// <summary>
        /// Test Method
        /// </summary>
        public void TestMethod1(string foo) { }

        /// <summary>
        /// Test Method
        /// </summary>
        public void TestMethod1<T>(T foo) { }
    }
}
