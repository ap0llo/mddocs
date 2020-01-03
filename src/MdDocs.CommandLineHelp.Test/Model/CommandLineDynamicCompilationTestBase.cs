using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using Grynwald.MdDocs.TestHelpers;
using Microsoft.CodeAnalysis;

namespace Grynwald.MdDocs.CommandLineHelp.Test.Model
{
    public abstract class CommandLineDynamicCompilationTestBase : DynamicCompilationTestBase
    {
        private static readonly Lazy<IReadOnlyList<MetadataReference>> s_MetadataReferences = new Lazy<IReadOnlyList<MetadataReference>>(() =>
        {
            // use all base references but add "CommandLine" assembly
            var references = s_DefaultMetadataReferences.Value.ToList();
            references.Add(MetadataReference.CreateFromFile(typeof(VerbAttribute).Assembly.Location));
            return references;

        });

        protected override IReadOnlyList<MetadataReference> GetMetadataReferences() => s_MetadataReferences.Value;
    }
}
