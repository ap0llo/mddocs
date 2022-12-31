using System.Collections.Generic;
using Cake.AzurePipelines.Module;
using Cake.Common.Tools.DotNet.Test;
using Cake.Core;
using Cake.DotNetLocalTools.Module;
using Cake.Frosting;
using Grynwald.SharedBuild;

return new CakeHost()
    // Usage of AzurePipelinesModule temporarily commented out since it does not yet support Cake 3.0
    //.UseModule<AzurePipelinesModule>()
    .UseModule<LocalToolsModule>()
    .InstallToolsFromManifest(".config/dotnet-tools.json")
    .UseSharedBuild<BuildContext>(
        // Load all tasks except the "Test" task (there is a customized version of the "Test" task defined below)
        taskFilter: task => task != typeof(Grynwald.SharedBuild.Tasks.TestTask)
    )
    .Run(args);


public class BuildContext : DefaultBuildContext
{
    public override IReadOnlyCollection<IPushTarget> PushTargets { get; } = new[]
    {
        new PushTarget(
            PushTargetType.MyGet,
            "https://www.myget.org/F/ap0llo-mddocs/api/v3/index.json",
            context => context.Git.IsMasterBranch || context.Git.IsReleaseBranch
        ),
        KnownPushTargets.NuGetOrg(isActive: context => context.Git.IsReleaseBranch)
    };

    public BuildContext(ICakeContext context) : base(context)
    { }
}

/// <summary>
/// Customized "Test" task
/// </summary>
[TaskName(TaskNames.Test)]
[IsDependentOn(typeof(Grynwald.SharedBuild.Tasks.PackTask))]
public class TestTask : Grynwald.SharedBuild.Tasks.TestTask
{
    protected override DotNetTestSettings GetDotNetTestSettings(IBuildContext context)
    {
        var testSettings = base.GetDotNetTestSettings(context);

        // The test project "Grynwald.MdDocs.BuildVerification" requires access to the NuGet package output directory
        // which is passed in as environment variable
        testSettings.EnvironmentVariables["MDDOCS_TEST_PACKAGEOUTPUTPATH"] = context.Output.PackagesDirectory.FullPath;

        return testSettings;
    }
}
