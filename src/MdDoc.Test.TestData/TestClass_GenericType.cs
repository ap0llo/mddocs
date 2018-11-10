using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Test.TestData
{
    /// <summary>
    /// Test class 2
    /// </summary>
    public class TestClass_GenericType<T>
    {
        /// <summary>
        /// Test method 1
        /// </summary>
        public void TestMethod1(T foo) { throw new NotImplementedException(); }

        /// <summary>
        /// Test method 2
        /// </summary>
        public void TestMethod2<T2>(T2 foo) { throw new NotImplementedException(); }

        /// <summary>
        /// Test method 3
        /// </summary>
        public void TestMethod3<T2>(T2 foo, T bar) { throw new NotImplementedException(); }

    }
}
