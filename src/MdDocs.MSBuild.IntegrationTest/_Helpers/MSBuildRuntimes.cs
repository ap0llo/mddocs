using System;
using System.Collections.Generic;

namespace Grynwald.MdDocs.MSBuild.IntegrationTest
{
    public static class MSBuildRuntimes
    {
        public static readonly MSBuildRuntimeInfo DotNet6SDK = new(MSBuildRuntimeType.Core, Version.Parse("6.0.400"));

        public static readonly MSBuildRuntimeInfo DotNet7SDK = new(MSBuildRuntimeType.Core, Version.Parse("7.0.100"));

        public static readonly MSBuildRuntimeInfo VisualStudio2022 = new(MSBuildRuntimeType.Full, Version.Parse("17.0"));

        public static readonly MSBuildRuntimeInfo Default = DotNet7SDK;


        public static IEnumerable<MSBuildRuntimeInfo> All { get; } = new[]
        {
            DotNet6SDK,
            DotNet7SDK,
            VisualStudio2022
        };
    }
}
