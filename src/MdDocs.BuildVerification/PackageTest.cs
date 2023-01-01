using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grynwald.MdDocs.TestHelpers;
using VerifyXunit;
using Xunit;
using static VerifyXunit.Verifier;

namespace Grynwald.MdDocs.BuildVerification
{
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
