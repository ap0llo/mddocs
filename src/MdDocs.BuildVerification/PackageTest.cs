using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using VerifyXunit;
using Xunit;
using Xunit.Sdk;
using static VerifyXunit.Verifier;

namespace Grynwald.MdDocs.BuildVerification
{
    public class PackageInfo
    {
        public class FrameworkReference
        {
            public required string TargetFramework { get; init; }

            public required IReadOnlyList<string> Assemblies { get; init; }


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

        public required bool IsDevelopmentDependency { get; init; }

        public required IReadOnlyList<string> PackageTypes { get; init; }

        public required IEnumerable<PackageDependencyGroup> Dependencies { get; init; }

        public required IReadOnlyList<FrameworkReference> FrameworkReferences { get; init; }

        public required IReadOnlyList<string> Files { get; init; }


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

    public class PackagesFixture
    {
        public IReadOnlyCollection<PackageInfo> Packages { get; }


        public PackagesFixture()
        {
            var packageOutputPath = Environment.GetEnvironmentVariable("MDDOCS_TEST_PACKAGEOUTPUTPATH");

            if (String.IsNullOrEmpty(packageOutputPath))
            {
                throw new XunitException(
                    $"Tests in {nameof(PackageTest)} cannot be run unless environment variable MDDOCS_TEST_PACKAGEOUTPUTPATH is set. " +
                    $"Please set variable manually or run tests via the build script build.ps1");
            }

            if (!Directory.Exists(packageOutputPath))
            {
                throw new XunitException(
                    $"Package directory '{packageOutputPath}' from environment variable MDDOCS_TEST_PACKAGEOUTPUTPATH does not exist. " +
                   $"Please set variable manually or run tests via the build script build.ps1"
                );
            }

            Packages = Directory
                .GetFiles(packageOutputPath, "*.nupkg", SearchOption.AllDirectories)
                .Select(PackageInfo.FromFile)
                .ToList();
        }


        public PackageInfo? TryGetPackage(string packageId)
        {
            return Packages.SingleOrDefault(x => x.Id.Equals(packageId, StringComparison.OrdinalIgnoreCase));
        }
    }

    [UsesVerify]
    public class PackageTest : IClassFixture<PackagesFixture>
    {
        private readonly PackagesFixture m_PackagesFixture;


        public PackageTest(PackagesFixture packagesFixture)
        {
            m_PackagesFixture = packagesFixture;
        }


        public static IEnumerable<string> KnownPackageIds => new[] { "Grynwald.MdDocs", "Grynwald.MdDocs.MSBuild" };

        public static IEnumerable<object[]> KnownPackageIdsTestData() => KnownPackageIds.Select(x => new object[] { x });


        [Fact]
        public void Building_the_solution_produces_the_expected_packages()
        {
            // ARRANGE

            // ACT
            var packageIds = m_PackagesFixture.Packages.Select(x => x.Id);

            // ASSERT

            Assert.Collection(
                packageIds.Order(),
                KnownPackageIds.Order().Select<string, Action<string>>(expected => actual => Assert.Equal(expected, actual)).ToArray()
            );
        }

        [Theory]
        [MemberData(nameof(KnownPackageIdsTestData))]
        public Task Package_has_the_expected_content(string packageId)
        {
            var package = m_PackagesFixture.TryGetPackage(packageId);
            Assert.NotNull(package);

            return Verify(package)
                .DontIgnoreEmptyCollections()
                .IgnoreMembers<PackageInfo>(nameof(PackageInfo.Identity))
                .IgnoreMembers<PackageInfo>(nameof(PackageInfo.PackageFilePath))
                .UseParameters(packageId);
        }
    }
}
