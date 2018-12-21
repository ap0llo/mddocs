using System;

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
