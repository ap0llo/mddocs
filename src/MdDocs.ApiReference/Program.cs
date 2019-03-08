using System;
using System.Diagnostics;
using System.IO;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Pages;

namespace Grynwald.MdDocs.ApiReference
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
