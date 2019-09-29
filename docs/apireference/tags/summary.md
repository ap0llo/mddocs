# `summary` Tag

The `summary` tag is intended for specifying a short explanation of the member
or type. In the generated documentation, the text will be rendered as "Summary"
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

For each member, only a single `summary` tag can be added.

## Usage

The `summary` tag has no attributes, the content can be any text mixed with
inline tags, e.g.

```xml
<summary>
Documentation text referring to another type, e.g. <see cref="System.String" />
</summary>
```

## See also

- [`remarks` Tag](./remarks.md)
- [\<summary\> (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/summary)
