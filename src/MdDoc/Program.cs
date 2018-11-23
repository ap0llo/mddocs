using MdDoc.Model;
using MdDoc.Pages;
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
        static void Main(string[] args)
        {
            var assemblyPath = args[0];
            var outDir = args[1];

            if (Directory.Exists(outDir))
                Directory.Delete(outDir, true);


            using (var assemblyDocumentation = AssemblyDocumentation.FromFile(assemblyPath))
            {
                var factory = new PageFactory(assemblyDocumentation, outDir);
                foreach (var page in factory.AllPages)
                {
                    page.Save();
                }
            }
            
            if (Debugger.IsAttached)
            {
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
