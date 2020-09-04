using System;
using Xunit.Abstractions;

namespace Grynwald.MdDocs.MSBuild.IntegrationTest
{
    public class MSBuildRuntimeInfo : IXunitSerializable
    {
        public MSBuildRuntimeType Type { get; private set; }

        public Version Version { get; private set; }

        public MSBuildRuntimeInfo(MSBuildRuntimeType runtimeType, Version version)
        {
            Type = runtimeType;
            Version = version;
        }

        [Obsolete("For use by Xunit only", true)]
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public MSBuildRuntimeInfo()
        { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.


        public void Deserialize(IXunitSerializationInfo info)
        {
            Type = info.GetValue<MSBuildRuntimeType>(nameof(Type));
            var versionString = info.GetValue<string>(nameof(Version));
            Version = Version.Parse(versionString);
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Type), Type);
            info.AddValue(nameof(Version), Version.ToString());
        }


        public override string ToString() => $"MSBuild {Type}, version {Version}";
    }
}
