# MdDocs

## Overview

[![Build Status](https://dev.azure.com/ap0llo/OSS/_apis/build/status/mddocs?branchName=master)](https://dev.azure.com/ap0llo/OSS/_build/latest?definitionId=11&branchName=master)
[![Renovate](https://img.shields.io/badge/Renovate-enabled-brightgreen)](https://renovatebot.com/)
[![Conventional Commits](https://img.shields.io/badge/Conventional%20Commits-1.0.0-yellow.svg)](https://conventionalcommits.org)

| Package                   | NuGet.org                                                                                                                      | MyGet                                                                                                                                                                               |
|---------------------------|--------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `Grynwald.MdDocs`         | [![NuGet](https://img.shields.io/nuget/v/Grynwald.MdDocs.svg)](https://www.nuget.org/packages/Grynwald.MdDocs)                 | [![MyGet](https://img.shields.io/myget/ap0llo-mddocs/vpre/Grynwald.MdDocs.svg?label=myget)](https://www.myget.org/feed/ap0llo-mddocs/package/nuget/Grynwald.MdDocs)                 |
| `Grynwald.MdDocs.MSBuild` | [![NuGet](https://img.shields.io/nuget/v/Grynwald.MdDocs.MSBuild.svg)](https://www.nuget.org/packages/Grynwald.MdDocs.MSBuild) | [![MyGet](https://img.shields.io/myget/ap0llo-mddocs/vpre/Grynwald.MdDocs.MSBuild.svg?label=myget)](https://www.myget.org/feed/ap0llo-mddocs/package/nuget/Grynwald.MdDocs.MSBuild) |

*MdDocs* is a tool to generate documentation in the form of Markdown documents.
It currently supports:

- Generating API reference from a .NET assembly and the corresponding XML
  documentation file
- Generating command line help for projects that use the
  [CommandLineParser package](https://www.nuget.org/packages/CommandLineParser/)
  for parsing
  
For an example of what the output looks like, have a look at the [demoprojects](docs/demoprojects) directory.

## Documentation

For documentation on installation and usage, please refer to the
corresponding sub-pages:

- [MdDocs .NET CLI Tool](./docs/net-cli-tool.md)
- [MdDocs MSBuild Integration](./docs/msbuild-integration.md)
- [Configuration](./docs/configuration/README.md)

## Building from source

ℹ This repository uses git submodules. Use `git clone --recursive` to check out submodules as well.

MdDocs is a .NET Core application and can be built using the .NET SDK 3.1 (see [global.json](./global.json))

```ps1
dotnet restore .\src\MdDocs.sln

dotnet build .\src\MdDocs.sln

dotnet pack .\src\MdDocs.sln
```

To run tests, the .NET SK 2.1 (version 2.1.800) and a installation of Visual Studio 2019 is requried as well.
(this only applies to the `MdDocs.MSBuild.IntegrationTest` project, all other test project should be executable with only the .NET Core 3.1 SDK).

```ps1
dotnet test .\src\MdDocs.sln
```

## Issues

If you run into any issues or if you are missing a feature, feel free
to open an [issue](https://github.com/ap0llo/mddocs/issues).

I'm also using issues as a backlog of things that come into my mind or
things I plan to implement, so don't be surprised if many issues were
created by me without anyone else being involved in the discussion.

## Acknowledgments

*MdDocs* was made possible through a number of libraries (aside from
.NET Core and .NET Standard). Thanks to all the people contribution to these projects:

- Parts of this program are based on code from the [NuDoq project](https://github.com/kzu/NuDoq/),
  licensed under the Apache License 2.0.
  See [`XmlDocsReader.cs`](src/MdDoc/Model/XmlDocs/XmlDocsReader.cs) for details
- [Mono.Cecil](https://github.com/jbevain/cecil/)
- [CommandLineParser](https://github.com/gsscoder/commandline)
- [Nerdbank.GitVersioning](https://github.com/AArnott/Nerdbank.GitVersioning/)
- [Microsoft.Extensions.Logging](https://github.com/aspnet/Extensions)
- [Microsoft.Extensions.Configuration](https://github.com/aspnet/Extensions)
- [Microsoft.DotNet.Analyzers.Compatibility](https://github.com/dotnet/platform-compat)
- [xUnit](http://xunit.github.io/)
- [Xunit.Combinatorial](https://github.com/AArnott/Xunit.Combinatorial)
- [Moq](https://github.com/moq/moq4)
- [ApprovalTests](https://github.com/approvals/ApprovalTests.Net)
- [Microsoft.CodeAnalysis.CSharp](https://github.com/dotnet/roslyn)
- [Coverlet](https://github.com/tonerdo/coverlet)
- [MSBuild](https://github.com/dotnet/msbuild/)
- [SourceLink](https://github.com/dotnet/sourcelink)
- [NuGet](https://github.com/NuGet/NuGet.Client)
- [Newtonsoft.Json](https://www.newtonsoft.com/json)

## Versioning and Branching

The version of the library is automatically derived from git and the information
in `version.json` using [Nerdbank.GitVersioning](https://github.com/AArnott/Nerdbank.GitVersioning):

- The master branch  always contains the latest version. Packages produced from
  master are always marked as pre-release versions (using the `-pre` suffix).
- Stable versions are built from release branches. Build from release branches
  will have no `-pre` suffix
- Builds from any other branch will have both the `-pre` prerelease tag and the git
  commit hash included in the version string

To create a new release branch use the [`nbgv` tool](https://www.nuget.org/packages/nbgv/)
(at least version `3.0.4-beta`):

```ps1
dotnet tool install --global nbgv --version 3.0.4-beta
nbgv prepare-release
```
