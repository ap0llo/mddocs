using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Test.TestData
{
    public struct TestStruct_InterfaceImplementation : IDisposable, TestInterface_Type
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
