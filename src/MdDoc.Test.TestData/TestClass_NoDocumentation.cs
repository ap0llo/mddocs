#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;

namespace MdDoc.Test.TestData
{
    public class TestClass_NoDocumentation
    {
        public string Field1;

        public event EventHandler Event1;

        public string Property1 { get; set; }

        public void Method1()
        {
        }

    }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
