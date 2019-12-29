#nullable disable

using System;

namespace Grynwald.MdDocs.ApiReference.Test.TestData
{
    /// <summary>
    /// Test class for events
    /// </summary>
    public class TestClass_Events
    {
        /// <summary>
        /// Event 1
        /// </summary>
        public event EventHandler Event1;

        /// <summary>
        /// Event 2
        /// </summary>
        public event EventHandler<EventArgs> Event2;

        /// <summary>
        /// Event 3
        /// </summary>
        public event EventHandler Event3
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }
    }
}
