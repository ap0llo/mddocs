using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit.Sdk;

namespace Grynwald.MdDocs.TestHelpers
{
    /// <summary>
    /// Test fixture that provides access to the NuGet packages in the build output
    /// </summary>
    public class PackagesFixture
    {
        private const string MDDOCS_TEST_PACKAGEOUTPUTPATH = "MDDOCS_TEST_PACKAGEOUTPUTPATH";


        public IReadOnlyCollection<PackageInfo> Packages { get; }


        public PackagesFixture()
        {
            var packageOutputPath = Environment.GetEnvironmentVariable(MDDOCS_TEST_PACKAGEOUTPUTPATH);

            if (String.IsNullOrEmpty(packageOutputPath))
            {
                throw new XunitException(
                    $"Tests accessing the NuGet packages in the build output cannot be run unless environment variable {MDDOCS_TEST_PACKAGEOUTPUTPATH} is set. " +
                    $"Please set variable manually or run tests via the build script build.ps1");
            }

            if (!Directory.Exists(packageOutputPath))
            {
                throw new XunitException(
                    $"Package directory '{packageOutputPath}' from environment variable {MDDOCS_TEST_PACKAGEOUTPUTPATH} does not exist. " +
                   $"Please set variable manually or run tests via the build script build.ps1"
                );
            }

            Packages = Directory
                .GetFiles(packageOutputPath, "*.nupkg", SearchOption.AllDirectories)
                .Select(PackageInfo.FromFile)
                .ToList();
        }


        public PackageInfo GetPackage(string packageId) => Packages.Single(x => x.Id.Equals(packageId, StringComparison.OrdinalIgnoreCase));
    }
}
