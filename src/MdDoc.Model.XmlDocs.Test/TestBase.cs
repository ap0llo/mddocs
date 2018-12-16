using MdDoc.Test.TestData;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MdDoc.Model.XmlDocs.Test
{
    public class TestBase
    {
        protected readonly XDocument m_TestData;

        public TestBase()
        {
            // This will load the XML Documentation files for the MdDoc.Test.TestData assembly
            // By default - this does not work with Visual Studio's Live Unit Testing
            // because it does not generate XML docs 
            // To enable this, go to "Tools" -> "Options" -> "Live Unit Testing" and
            // select "Enable debug symbol and xml documentation comment generation"
            // (see https://docs.microsoft.com/en-us/visualstudio/test/live-unit-testing?view=vs-2017#configure-live-unit-testing)

            var assemblyPath = new Uri(typeof(TestClass_XmlDocs<,>).Assembly.CodeBase).LocalPath;
            var docsPath = Path.ChangeExtension(assemblyPath, ".xml");

            m_TestData = XDocument.Load(docsPath);            
        }


        protected XElement GetMember(string name)
        {
            return m_TestData.Root
                .Element("members")
                .Elements("member")
                .Single(x => x.Attribute("name")?.Value == name);
        }

    }
}
