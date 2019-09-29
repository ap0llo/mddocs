# `returns` Tag

The `returns` tag allows documenting a method's return value.

## Supported members

| Member kind | Supported |
|-------------|-----------|
| Types       | No        |
| Fields      | No        |
| Events      | No        |
| Properties  | No        |
| Indexers    | No        |
| Methods     | Yes       |

For each member, only a single `returns` tag can be added.

## Usage

The `returns` tag has no attributes, the content can be any text mixed with
inline tags, e.g.

```xml
<returns>Documentation text referring to another type, e.g. <see cref="System.String" /></returns>
```

## See also

- [`value` Tag](./value.md)
- [\<returns\> (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/returns)
