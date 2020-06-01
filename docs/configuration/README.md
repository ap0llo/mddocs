# Configuration

**Applies to:** Version 0.4 an later

The behaviour of MdDocs can be customized by changing the configuration.
Most settings are optional and MdDocs aims to use sensible default values or all of them.

## Configuration Sources

Settings are loaded from a number of *setting sources*:

1. Default settings 
1. Configuration file
1. Execution parameters
   - Commandline parameters in the case of the [MdDocs .NET CLI Tool](../net-cli-tool.md)
   - MSBuild properties in the case of the [MdDocs MSBuild targets](../msbuild-integration.md)

### Default settings

The default settings are embedded in the MdDocs executable and apply, if no other source specifies a specific setting.

The default settings are defined in [`defaultSettings.json`](../../src/MdDocs.Common/Configuration/defaultSettings.json).

### Configuration file

You can customize settings by placing them in a configuration file.
The configuration file is a JSON file and uses the same schema as [`defaultSettings.json`](../../src/MdDocs.Common/Configuration/defaultSettings.json).

The use of a configuration file is **optional**.

You can specify the path of the configuration file using the `configurationFilePath` commandline parameter respectively the `MdDocsConfigurationFilePath` MSBuild property.
(see [Commandline reference](../commandline/index.md)).

## Settings

Setting names in the following table are separated by `:` which denote keys and sub-keys the JSON configuration file.
For example setting `key:subkey` to `value` would need to be specified in the configuration file like this:

```json
{
    "key" : {
        "subkey" : "value"
    }
}
```

## Available Settings

- [API Reference](./apireference/README.md)
  - [Assembly Path](./apireference/README.md#assembly-path)
  - [Output Path](./apireference/README.md#output-path)
  - [Markdown Preset](./apireference/README.md#markdown-preset)
- [Command Line Help](./commandlinehelp/README.md)
  - [Assembly Path](./commandlinehelp/README.md#assembly-path)
  - [Output Path](./commandlinehelp/README.md#output-path)
  - [Include Version](./commandlinehelp/README.md#include-version)
  - [Markdown Preset](./commandlinehelp/README.md#markdown-preset)

## See Also

- [MdDocs .NET CLI Tool](../net-cli-tool.md)
- [MdDocs MSBuild Integration](../msbuild-integration.md)
- [API Reference Configuration](./apireference/README.md)
- [Command Line Help Configuration](./commandlinehelp/README.md)