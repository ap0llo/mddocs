# `example` Tag

The `example` tag is intended for documenting examples on how to use
a type or member, e.g. by providing sample code. In the generated documentation,
the text will be rendered as "Example" section.

## Supported members

| Member kind | Supported |
|-------------|-----------|
| Types       | Yes       |
| Fields      | Yes       |
| Events      | Yes       |
| Properties  | Yes       |
| Indexers    | Yes       |
| Methods     | Yes       |

For each member, only a single `example` tag can be added.

## Usage

The `example` tag has no attributes, the content can be any text mixed with
inline tags, e.g.

```xml
<example>
Documentation text referring to another type, e.g. <see cref="System.String" />
</example>
```

## See also

- [\<example\> (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/example)
