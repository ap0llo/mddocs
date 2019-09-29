# `value` Tag

The `value` tag allows documenting the value of a property or indexer.

## Supported members

| Member kind | Supported |
|-------------|-----------|
| Types       | No        |
| Fields      | Yes       |
| Events      | No        |
| Properties  | Yes       |
| Indexers    | Yes       |
| Methods     | No        |

For each member, only a single `value` tag can be added.

## Usage

The `value` tag has no attributes, the content can be any text mixed with
inline tags, e.g.

```xml
<value>Documentation text referring to another type, e.g. <see cref="System.String" /></summary>
```

## See also

- [`returns` Tag](./returns.md)
- [\<value\> (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/value)
