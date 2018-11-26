using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MdDoc.Test.TestData
{
    class TestClass_TypeFormatter
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

        public Stream Property16 { get; set; }

        public string[] Property17 { get; set; }

        public Stream[] Property18 { get; set; }
        
        public IEnumerable<string> Property19 { get; set; }

        public IEnumerable<Stream> Property20 { get; set; }

        public Dictionary<string, Stream> Property21 { get; set; }

    }

    class TestClass_TypeFormatter<T1, T2>
    {
        public IEnumerable<T1> Property1 { get; set; }

        public IEnumerable<T2> Property2 { get; set; }

        public Dictionary<T1, T2> Property3 { get; set; }
    }
}
