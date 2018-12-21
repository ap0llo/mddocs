using System;
using System.Collections.Generic;
using System.IO;

namespace MdDoc.Test.TestData
{
    public class TestClass_Properties_CSharpDefinition
    {                
        public int Property1 { get; set; }

        public byte Property2 { get; set; }

        public string Property3 { get; }

        public string Property4 { get; private set; }

        public string Property5 { private get; set; }

        public Stream Property6 { get; }

        public IEnumerable<string> Property7 { get; }

        public int this[object parameter] { get { throw new NotImplementedException(); } }

        public int this[object parameter1, Stream parameter2] { get { throw new NotImplementedException(); } }
    }
}
