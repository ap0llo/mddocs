# MdDocs

## Overview

*MdDocs* is a tool to generate documentation in the form of Markdown documents.
It currently supports generating API reference from a .NET assembly and the corresponding XML documentation file.

For an example of what the output looks like, have a look at the [DemoProject](docs/api/DemoProject/Namespace.md) docs.

## Installation

*MdDocs* is a .NET Core tool distributed as NuGet package.

- Prerelease builds are available on [MyGet](https://www.myget.org/feed/ap0llo-mddocs/package/nuget/Grynwald.Utilities)
- Release versions are available on [NuGet.org](https://www.nuget.org/packages/Grynwald.MdDocs)

To install the tool globally, run

```ps1
dotnet tool install --global Grynwald.MdDocs
```

## Usage

To generate API reference documentation for a .NET assembly, run

```ps1
mddocs apireference --assembly <PATH-TO-ASSEMBLY> --outdir <OUTPUT-DIRECTORY>
```

**Note:** The tool will delete all files in the output directory before writing the result.

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
