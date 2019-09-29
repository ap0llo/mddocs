# `remarks` Tag

The `remarks` tag is intended for specifying detailed documentation of a member
or type. In the generated documentation, the text will be rendered as "Remarks"
section.

## Supported members

| Member kind | Supported |
|-------------|-----------|
| Types       | Yes       |
| Fields      | Yes       |
| Events      | Yes       |
| Properties  | Yes       |
| Indexers    | Yes       |
| Methods     | Yes       |

For each member, only a single `remarks` tag can be added.

## Usage

The `Remarks` tag has no attributes, the content can be any text mixed with
inline tags, e.g.

```xml
<remarks>
Detailed documentation text with support for links, e.g. <see cref="System.String" />
</remarks>
```

## See also

- [`summary` Tag](./summary.md)
- [\<remarks\> (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/remarks)
