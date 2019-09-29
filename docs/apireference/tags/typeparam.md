# `typeparam` Tag

The `typeparam` tags allows adding a documentation to a type parameter
of a generic type or method.

In the generated documentation, the text will be included in the
documentation section for the corresponding type parameter.

## Supported members

| Member kind | Supported |
|-------------|-----------|
| Types       | Yes       |
| Fields      | No        |
| Events      | No        |
| Properties  | No        |
| Indexers    | Yes       |
| Methods     | Yes       |

For each member, there can be multiple `typeparam` tags (at most one tag
for every type parameter).

## Usage

The required attribute `name` specifies which type parameter the documentation
text applies to. The name must match the parameter name in the method
or indexer signature.

```xml
<typeparam name="T">Description of the generic type parameter</param>  
```

## See also

- [`param` Tag](./param.md)
- [\<typeparam\> (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/typeparam)
