# MdDocs

## Overview

[![Build Status](https://dev.azure.com/ap0llo/OSS/_apis/build/status/mddocs?branchName=master)](https://dev.azure.com/ap0llo/OSS/_build/latest?definitionId=11&branchName=master)

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

## Installation

*MdDocs* is a distributed as NuGet packages.

- Prerelease builds are available on [MyGet](https://www.myget.org/feed/ap0llo-mddocs/package/nuget/Grynwald.Utilities)
- Release versions are available on [NuGet.org](https://www.nuget.org/packages/Grynwald.MdDocs)

MdDocs can be used either as .NET Core (global) tool or integrated into
the project build as a set of MSBuild targets.

### Installing the .NET Core Tool

The package `Grynwald.MdDocs` provides MdDocs as a .NET Core (global) tool.
To install it, run

```ps1
dotnet tool install --global Grynwald.MdDocs
```

### Installing MSBuild integration

The package `Grynwald.MdDocs.MSBuild` provides allows generating documentation
as part of the build process. To install the package, run

```ps1
dotnet add package Grynwald.MdDocs.MSBuild
```

## Usage

### .NET Core Tool

See also: [Command Line Reference](docs/commandline/index.md)

#### API Reference

To generate API reference documentation for a .NET assembly, run:

```ps1
mddocs apireference --assembly <PATH-TO-ASSEMBLY> --outdir <OUTPUT-DIRECTORY>
```

**Note:** If the output directory already exists, all files in the output
directory will be deleted.

For an example of what the output looks like, have a look at the
[demo project](docs/demoprojects/api/DemoProject/index.md).

For a list of supported documentation tags, see
[Supported documentation tags](./docs/apireference/tags.md).

#### Command Line Help

To generate command line help for .NET console application implemented using
the [CommandLineParser package](https://www.nuget.org/packages/CommandLineParser/),
run:

```ps1
mddocs commandlinehelp --assembly <PATH-TO-ASSEMBLY> --outdir <OUTPUT-DIRECTORY>
```

**Note:** If the output directory already exists, all files in the output
directory will be deleted.

For an example of what the output looks like, have a look at the
[demo project](docs/demoprojects/commandline/index.md).

### MSBuild-integrated

When the MSBuild package is installed, documentation can be generated
by running the appropriate MSBuild targets. Optionally, the targets
can eb configured to automatically generate documentation when the
project is built.

#### API Reference

To generate API reference for a project, run

```ps1
dotnet msbuild <PROJECT> /t:GenerateApiReferenceDocumentation
```

The behaviour of the target can be customized by setting the following
MSBuild properties:

| Property Name                              | Default Value            | Description                                                                               |
|--------------------------------------------|--------------------------|-------------------------------------------------------------------------------------------|
| `ApiReferenceDocumentationOutputPath`      | `$(OutputPath)docs/api/` | The directory to save the generated documetation to.                                      |
| `GenerateApiReferenceDocumentationOnBuild` | `false`                  | Set the to `true` to automatically generate API documentation when the project is built.  |
| `MdDocsMarkdownPreset`                     | `Default`                | Specify the "preset" to use for the generated markdown. Valid values: `Default`, `MkDocs` |

**Note:** If the output directory already exists, all files in the output
directory will be deleted.

For an example of what the output looks like, have a look at the
[demo project](docs/demoprojects/api/DemoProject/index.md).

For a list of supported documentation tags, see
[Supported documentation tags](./docs/apireference/tags.md).

#### Command Line Help

To generate command line documentation for a project, run

```ps1
dotnet msbuild <PROJECT> /t:GenerateCommandLineDocumentation
```

The behaviour of the target can be customized by setting the following
MSBuild properties:

| Property Name                             | Default Value                    | Description                                                                                   |
|-------------------------------------------|----------------------------------|-----------------------------------------------------------------------------------------------|
| `CommandLineDocumentationOutputPath`      | `$(OutputPath)docs/commandline/` | The directory to save the generated documetation to.                                          |
| `GenerateCommandLineDocumentationOnBuild` | `false`                          | Set to `true` to automatically generate command line documentation when the project is built. |
| `MdDocsMarkdownPreset`                    | `Default`                        | Specify the "preset" to use for the generated markdown. Valid values: `Default`, `MkDocs`     |

**Note:** If the output directory already exists, all files in the output
directory will be deleted.

For an example of what the output looks like, have a look at the
[demo project](docs/demoprojects/commandline/index.md).

## Building from source

MdDocs is a .NET Core application and can be built using the .NET SDK 2.1

```ps1
  dotnet restore .\src\MdDocs.sln

  dotnet build .\src\MdDocs.sln

  dotnet pack .\src\MdDocs.sln
```

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
- [Microsoft.DotNet.Analyzers.Compatibility](https://github.com/dotnet/platform-compat)
- [xUnit](http://xunit.github.io/)
- [Xunit.Combinatorial](https://github.com/AArnott/Xunit.Combinatorial)
- [Moq](https://github.com/moq/moq4)
- [ApprovalTests](https://github.com/approvals/ApprovalTests.Net)

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
