using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Test.TestData
{
    /// <summary>
    /// Test class 4
    /// </summary>
    public class TestClass_Properties
    {

        //TODO: Properties, Indexers

        /// <summary>
        /// Property 1
        /// </summary>
        public int Property1 { get; set; }

        /// <summary>
        /// Property 2
        /// </summary>
        public int Property2 { get; }

        /// <summary>
        /// Indexer 1
        /// </summary>
        public int this[int foo] { get { throw new NotImplementedException(); }  }

        /// <summary>
        /// Indexer 2
        /// </summary>
        public int this[int foo, double bar] { get { throw new NotImplementedException(); } }
    }
}
