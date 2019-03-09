using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Model.Test.XmlDocs
{
    public class XmlDocsReaderTest
    {
        [Theory, CombinatorialData]
        public void GetCodeLanguage_returns_expected_language_when_value_can_be_parsed_using_Enum_Parse(CodeLanguage value)
        {
            Assert.Equal(value, XmlDocsReader.GetCodeLanguage(value.ToString(), NullLogger.Instance));
            Assert.Equal(value, XmlDocsReader.GetCodeLanguage(value.ToString().ToUpper(), NullLogger.Instance));
            Assert.Equal(value, XmlDocsReader.GetCodeLanguage(value.ToString().ToLower(), NullLogger.Instance));
        }

        [Theory]
        [InlineData("cs", CodeLanguage.CSharp)]
        [InlineData("c#", CodeLanguage.CSharp)]
        [InlineData("CSharp", CodeLanguage.CSharp)]
        [InlineData("cpp", CodeLanguage.CPlusPlus)]
        [InlineData("c++", CodeLanguage.CPlusPlus)]
        [InlineData("CPlusPlus", CodeLanguage.CPlusPlus)]
        [InlineData("fs", CodeLanguage.FSharp)]
        [InlineData("f#", CodeLanguage.FSharp)]
        [InlineData("FSharp", CodeLanguage.FSharp)]
        [InlineData("fscript", CodeLanguage.FSharp)]
        [InlineData("EcmaScript", CodeLanguage.Javascript)]
        [InlineData("js", CodeLanguage.Javascript)]
        [InlineData("javascript", CodeLanguage.Javascript)]
        [InlineData("vb", CodeLanguage.VisualBasic)]
        [InlineData("vb#", CodeLanguage.VisualBasic)]
        [InlineData("vbnet", CodeLanguage.VisualBasic)]
        [InlineData("VB.NET", CodeLanguage.VisualBasic)]
        [InlineData("vbs", CodeLanguage.VisualBasicScript)]
        [InlineData("vbscript", CodeLanguage.VisualBasicScript)]
        [InlineData("htm", CodeLanguage.HTML)]
        [InlineData("html", CodeLanguage.HTML)]
        [InlineData("xml", CodeLanguage.XML)]
        [InlineData("xsl", CodeLanguage.XML)]
        [InlineData("xaml", CodeLanguage.XAML)]
        [InlineData("jsharp", CodeLanguage.JSharp)]
        [InlineData("J#", CodeLanguage.JSharp)]
        [InlineData("sql", CodeLanguage.SQL)]
        [InlineData("sql server", CodeLanguage.SQL)]
        [InlineData("sqlserver", CodeLanguage.SQL)]
        [InlineData("py", CodeLanguage.Python)]
        [InlineData("python", CodeLanguage.Python)]                
        [InlineData("ps1", CodeLanguage.Powershell)]
        [InlineData("powershell", CodeLanguage.Powershell)]
        [InlineData("pshell", CodeLanguage.Powershell)]
        [InlineData("bat", CodeLanguage.Batch)]
        [InlineData("batch", CodeLanguage.Batch)]
        public void GetCodeLanguage_returns_expected_language_for_names_supported_by_sandcastle(string name, CodeLanguage expectedLanguage)
        {
            // Test ensures, that languages names as used by SandCastle are parsed correctly.
            // For list of SandCastle language ids, see
            // http://ewsoftware.github.io/XMLCommentsGuide/html/1abd1992-e3d0-45b4-b43d-91fcfc5e5574.htm
            //
            //  Note: while SandCastle does not differentiate between HTML and XML, 
            //  the CodeLanguage Enum does
            //

            Assert.Equal(expectedLanguage, XmlDocsReader.GetCodeLanguage(name, NullLogger.Instance));
            Assert.Equal(expectedLanguage, XmlDocsReader.GetCodeLanguage(name.ToUpper(), NullLogger.Instance));
            Assert.Equal(expectedLanguage, XmlDocsReader.GetCodeLanguage(name.ToLower(), NullLogger.Instance));
        }


    }
}
