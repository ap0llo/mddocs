using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Packaging;
using NuGet.Packaging.Core;

namespace Grynwald.MdDocs.TestHelpers
{
    /// <summary>
    /// Encapsulates information about a NuGet package
    /// </summary>
    public class PackageInfo
    {
        public class FrameworkReference
        {
            public string TargetFramework { get; set; } = "";

            public IReadOnlyList<string> Assemblies { get; set; } = Array.Empty<string>();


            public static FrameworkReference FromFrameworkSpecificGroup(FrameworkSpecificGroup frameworkSpecificGroup)
            {
                return new FrameworkReference()
                {
                    TargetFramework = frameworkSpecificGroup.TargetFramework.DotNetFrameworkName,
                    Assemblies = frameworkSpecificGroup.Items.ToList()
                };
            }
        }

        public string PackageFilePath { get; }

        public string Id => Identity.Id;

        public PackageIdentity Identity { get; }

        public bool IsDevelopmentDependency { get; set; }

        public IReadOnlyList<string> PackageTypes { get; set; } = Array.Empty<string>();

        public IEnumerable<PackageDependencyGroup> Dependencies { get; set; } = Array.Empty<PackageDependencyGroup>();

        public IReadOnlyList<FrameworkReference> FrameworkReferences { get; set; } = Array.Empty<FrameworkReference>();

        public IReadOnlyList<string> Files { get; set; } = Array.Empty<string>();


        private PackageInfo(PackageIdentity identity, string packageFilePath)
        {
            Identity = identity;
            PackageFilePath = packageFilePath;
        }


        public static PackageInfo FromFile(string packageFilePath)
        {
            using var packageReader = new PackageArchiveReader(packageFilePath);

            return new PackageInfo(packageReader.GetIdentity(), packageFilePath)
            {
                IsDevelopmentDependency = packageReader.GetDevelopmentDependency(),
                Dependencies = packageReader.GetPackageDependencies(),
                PackageTypes = packageReader.GetPackageTypes().Select(x => x.Name).ToList(),
                Files = GetPackageFiles(packageReader).ToList(),
                FrameworkReferences = packageReader.GetFrameworkItems().Select(FrameworkReference.FromFrameworkSpecificGroup).ToList()
            };
        }


        private static IEnumerable<string> GetPackageFiles(IPackageCoreReader packageReader)
        {
            var files = packageReader.GetFiles();

            files = files.Except(new[]
            {
                "_rels/.rels",
                $"{packageReader.GetIdentity().Id}.nuspec",
                "[Content_Types].xml"
            });

            files = files.Where(x => !x.StartsWith("package/"));

            return files;
        }
    }
}
