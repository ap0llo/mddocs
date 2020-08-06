# Default Template

## Enabling the template

To use the default template, set the [*Template Name*](../README.md#template-name) setting to `Default`.
To customize the output produced by the default template, adjust the settings documented below.

## Settings

### Markdown Preset

<table>
    <tr>
        <td><b>Setting</b></td>
        <td><code>mddocs:apireference:template:default:markdownPreset</code></td>
    </tr>
    <tr>
        <td><b>Commandline Parameter</b></td>
        <td>-</td>
    </tr>
    <tr>
        <td><b>MSBuild Property</b></td>
        <td>-</td>
    </tr>
    <tr>
        <td><b>Default value</b></td>
        <td><code>Default</code></td>
    </tr>
    <tr>
        <td><b>Allowed values</b></td>
        <td>
            <ul>
                <li><code>Default</code></li>
                <li><code>MkDocs</code></li>
            </ul>
        </td>
    </tr>
</table>

The *Markdown Preset* customizes serialization of Markdown.

Supported values are:

- `default`: Produces Markdown that should work in most environments, including GitHub and GitLab
- `MkDocs`: Produces Markdown optimized for being rendered by Python-Markdown and [MkDocs](https://www.mkdocs.org/)

For details on the differences between the presets, see also [Markdown Generator docs](https://github.com/ap0llo/markdown-generator/blob/master/docs/apireference/Grynwald/MarkdownGenerator/MdSerializationOptions/Presets/index.md).

### Include Version

<table>
    <tr>
        <td><b>Setting</b></td>
        <td><code>mddocs:apireference.template:default:includeVersion</code></td>
    </tr>
    <tr>
        <td><b>Commandline Parameter</b></td>
        <td>-</td>
    </tr>
    <tr>
        <td><b>MSBuild Property</b></td>
        <td>-</td>
    </tr>
    <tr>
        <td><b>Default value</b></td>
        <td><code>true</code></td>
    </tr>
    <tr>
        <td><b>Allowed values</b></td>
        <td>
            <ul>
                <li><code>true</code></li>
                <li><code>false</code></li>
            </ul>
        </td>
    </tr>
</table>

The *Include Version* setting controls whether the version of the assembly is included in the generated documentation.

The version included in the documentation is the *informational version* if the assembly has a `AssemblyInformationalVersion` attribute, otherwise the assembly version is used.

### Include AutoGenerated Notice

<table>
    <tr>
        <td><b>Setting</b></td>
        <td><code>mddocs:apireference:template:default:includeAutoGeneratedNotice</code></td>
    </tr>
    <tr>
        <td><b>Commandline Parameter</b></td>
        <td>-</td>
    </tr>
    <tr>
        <td><b>MSBuild Property</b></td>
        <td>-</td>
    </tr>
    <tr>
        <td><b>Default value</b></td>
        <td><code>true</code></td>
    </tr>
    <tr>
        <td><b>Allowed values</b></td>
        <td>
            <ul>
                <li><code>true</code></li>
                <li><code>false</code></li>
            </ul>
        </td>
    </tr>
</table>

By default, Markdown documents generated by MdDocs include a comment at the start of the document that indicates that the file was auto-generated.
This notice is intended to avoid edits in files that will be lost when the file is regenerated.

The *Include AutoGenerated Notice* setting can be used to disable inclusion of this comment (by default, the comment is included):

```json
{
    "mddocs" : {
        "apireference" : {
            "template" :{
                "default" : {
                    "includeAutoGeneratedNotice" : false
                }
            }
        }
    }
}
```

## See Also

- [API Reference Configuration](../README.md)