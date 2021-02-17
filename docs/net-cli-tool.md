# MdDocs .NET CLI tool

**Applies to:** Version 0.5 and later

## Installation

*MdDocs* is a distributed as NuGet packages.

- Prerelease builds are available on [MyGet](https://www.myget.org/feed/ap0llo-mddocs/package/nuget/Grynwald.Utilities)
- Release versions are available on [NuGet.org](https://www.nuget.org/packages/Grynwald.MdDocs)

MdDocs can be used either as .NET Core CLI tool or integrated into
the project build as a set of MSBuild targets (see [MdDocs MSBuild-integration](./msbuild-integration.md)).

[![NuGet](https://img.shields.io/nuget/v/Grynwald.MdDocs.svg)](https://www.nuget.org/packages/Grynwald.MdDocs)

The package `Grynwald.MdDocs` provides MdDocs as a .NET Core (global) tool.
To install it, run

```ps1
dotnet tool install --global Grynwald.MdDocs
```

## Usage

See also: [Command Line Reference](./commandline/index.md)

### API Reference

To generate API reference documentation for one or more .NET assemblies, run:

```txt
mddocs apireference --assemblies <PATH-TO-ASSEMBLY-1> <PATH-TO-ASSEMBLY-2> --outdir <OUTPUT-DIRECTORY>
```

**⚠️ Note:** If the output directory already exists, all files in the output directory will be deleted.

- For an example of what the output looks like, have a look at the [demo project](./demoprojects/api/DemoProject/index.md).
- For a list of supported documentation tags, see [Supported documentation tags](./apireference/tags.md).

### Command Line Help

To generate command line help for .NET console application implemented using
the [CommandLineParser package](https://www.nuget.org/packages/CommandLineParser/),
run:

```txt
mddocs commandlinehelp --assembly <PATH-TO-ASSEMBLY> --outdir <OUTPUT-DIRECTORY>
```

**⚠️ Note:** If the output directory already exists, all files in the output directory will be deleted.

For an example of what the output looks like, have a look at the [demo project](./demoprojects/commandline/index.md).

## See Also

- [MdDocs MSBuild-integration](./msbuild-integration.md)
- [Command Line Reference](./commandline/index.md)
- [Configuration](./configuration/README.md)
