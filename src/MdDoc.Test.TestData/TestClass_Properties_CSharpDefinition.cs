using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MdDoc.Test.TestData
{
    public class TestClass_Properties_CSharpDefinition
    {                
        public int Property1 { get; set; }

        public byte Property2 { get; set; }

        public sbyte Property3 { get; set; }

        public char Property4 { get; set; }

        public decimal Property5 { get; set; }

        public double Property6 { get; set; }

        public float Property7 { get; set; }

        public bool Property8 { get; set; }

        public uint Property9 { get; set; }

        public long Property10 { get; set; }

        public ulong Property11 { get; set; }

        public object Property12 { get; set; }

        public short Property13 { get; set; }

        public ushort Property14 { get; set; }

        public string Property15 { get; set; }

        public string Property16 { get; }

        public string Property17 { get; private set; }

        public string Property18 { private get; set; }

        public Stream Property19 { get; }
        
        public int this[object parameter] { get { throw new NotImplementedException(); } }

        public int this[object parameter1, Stream parameter2] { get { throw new NotImplementedException(); } }
    }
}
