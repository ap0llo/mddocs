using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Test.TestData
{
    public class TestClass_InterfaceImplementation : TestInterface_Type, IDisposable
    {      
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
