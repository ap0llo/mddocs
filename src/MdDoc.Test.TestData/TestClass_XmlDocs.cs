using System;

namespace MdDoc.Test.TestData
{
    /// <summary>
    /// This is a test class for testing parsing of XML documentation
    /// </summary>
    /// <remarks>Remarks</remarks>
    /// <example>Example1</example>    
    /// <typeparam name="T1">Type parameter 1</typeparam>
    /// <typeparam name="T2">Type parameter 2</typeparam>
    public class TestClass_XmlDocs<T1, T2>
    {
        /// <summary>
        /// Summary
        /// </summary>
        /// <remarks>Remarks</remarks>
        /// <example>Example</example>
        /// <value>Value</value>
        public string Field1;


        /// <summary>
        /// Summary
        /// </summary>
        /// <remarks>Remarks</remarks>
        /// <example>Example</example>
        /// <value>Value</value>
        /// <exception cref="ArgumentException">Exception 1</exception>
        public string Property1 { get; }


        /// <summary>
        /// Summary
        /// </summary>
        /// <remarks>Remarks</remarks>
        /// <example>Example</example>
        public event EventHandler Event1;


        /// <summary>
        /// Line1
        /// </summary>
        /// <remarks>Remarks</remarks>
        /// <example>Example</example>
        /// <exception cref="InvalidOperationException">Exception 1</exception>
        /// <exception cref="ArgumentException">Exception 2</exception>
        /// <param name="parameter1">Documentation for parameter 1</param>
        /// <returns>The method returns an object</returns>
        public object TestMethod01(string parameter1) => null;

        /// <summary>
        /// Line1
        /// Line2
        /// </summary>
        public void TestMethod02() { }

        /// <summary>
        /// Line1
        /// 
        /// Line2
        /// </summary>
        public void TestMethod03() { }

        /// <summary>
        /// Line1
        /// <see cref="TestClass_Events"/>
        /// Line2
        /// </summary>
        public void TestMethod04() { }

        /// <summary>
        /// Content <see cref="TestClass_Events"/> Content
        /// </summary>
        public void TestMethod05() { }
    }
}
