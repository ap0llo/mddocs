using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MdDoc
{
    class Program
    {
        const string s_Path = @"C:\Users\Andreas\Desktop\grynwald.markdowngenerator.2.0.78\lib\netstandard2.0\Grynwald.MarkdownGenerator.dll";
        const string s_OutDir = @"C:\Users\Andreas\Desktop\tmp";

        static void Main(string[] args)
        {
            if (Directory.Exists(s_OutDir))
                Directory.Delete(s_OutDir, true);


            var documentationWriter = new DocumentationWriter(s_Path, s_OutDir);
            documentationWriter.SaveDocumentation();

            
            if (Debugger.IsAttached)
            {
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
