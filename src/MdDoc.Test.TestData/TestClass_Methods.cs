using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Test.TestData
{
    /// <summary>
    /// Test class for methods
    /// </summary>
    public class TestClass_Methods
    {

        /// <summary>
        /// Test Method 1
        /// </summary>
        public void TestMethod1() { }


        /// <summary>
        /// Test Method 2
        /// </summary>
        public void TestMethod2(string foo) { }
       
        /// <summary>
        /// Test Method 3
        /// </summary>
        public string TestMethod3(string foo) { throw new NotImplementedException(); }

        /// <summary>
        /// Test Method 4
        /// </summary>
        public string TestMethod4(string foo, string bar) { throw new NotImplementedException(); }

        /// <summary>
        /// Test Method 5
        /// </summary>
        public string TestMethod5<T>(T foo, string bar) { throw new NotImplementedException(); }

        /// <summary>
        /// Test Method 6
        /// </summary>
        public string TestMethod6(IEnumerable<string> bar) { throw new NotImplementedException(); }

        /// <summary>
        /// Test Method 7
        /// </summary>
        public string TestMethod7<T1, T2>(T1 foo, T2 bar) { throw new NotImplementedException(); }

        /// <summary>
        /// Test Method 8
        /// </summary>
        public T2 TestMethod8<T1, T2>(T1 foo, T2 bar) { throw new NotImplementedException(); }

        /// <summary>
        /// Test Method 9
        /// </summary>
        public T2 TestMethod9<T1, T2>(T2 foo, T1 bar) { throw new NotImplementedException(); }


        /// <summary>
        /// Test Method 10
        /// </summary>
        public void TestMethod10(string[] parameter) { }

        /// <summary>
        /// Test Method 11
        /// </summary>
        public void TestMethod11(string[][] parameter) { }


        /// <summary>
        /// Test Method 12
        /// </summary>
        public void TestMethod12(string[,] parameter) { }

    }
}
