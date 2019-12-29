# MdDocs MSBuild Integration

## Installation

*MdDocs* is a distributed as NuGet packages.

- Prerelease builds are available on [MyGet](https://www.myget.org/feed/ap0llo-mddocs/package/nuget/Grynwald.Utilities)
- Release versions are available on [NuGet.org](https://www.nuget.org/packages/Grynwald.MdDocs)

MdDocs can be used either as .NET Core (global) tool (see
[MdDocs .NET CLI tool](./net-cli-tool.md)) or integrated into
the project build as a set of MSBuild targets.

[![NuGet](https://img.shields.io/nuget/v/Grynwald.MdDocs.MSBuild.svg)](https://www.nuget.org/packages/Grynwald.MdDocs.MSBuild)

The package `Grynwald.MdDocs.MSBuild` allows generating documentation
as part of the build process. To install the package, run

```ps1
dotnet add package Grynwald.MdDocs.MSBuild
```

or install it through the Visual Studio NuGet package manager

## Usage

When the MSBuild package is installed, documentation can be generated
by running the appropriate MSBuild targets. Optionally, the targets
can be configured to automatically generate documentation when the
project is built.

### API Reference

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
[demo project](./demoprojects/api/DemoProject/index.md).

For a list of supported documentation tags, see
[Supported documentation tags](./apireference/tags.md).

### Command Line Help

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
[demo project](./demoprojects/commandline/index.md).
