using System;

namespace Grynwald.MdDocs.ApiReference.Test.TestData
{
    /// <summary>
    /// Test class for events
    /// </summary>
    public interface TestInterface_Events
    {
        /// <summary>
        /// Event 1
        /// </summary>
        event EventHandler Event1;
        
        /// <summary>
        /// Event 2
        /// </summary>
        event EventHandler<EventArgs> Event2;

        /// <summary>
        /// Event 3
        /// </summary>
        event EventHandler Event3;        
    }
}
