namespace MdDoc.Test.TestData
{
    /// <summary>
    /// Test class 4
    /// </summary>
    public interface TestInterface_Properties
    {        
        /// <summary>
        /// Property 1
        /// </summary>
        int Property1 { get; set; }

        /// <summary>
        /// Property 2
        /// </summary>
        int Property2 { get; }

        
        /// <summary>
        /// Indexer 1
        /// </summary>
        int this[int foo] { get;  }

        /// <summary>
        /// Indexer 2
        /// </summary>
        int this[int foo, double bar] { get; }
    }
}
