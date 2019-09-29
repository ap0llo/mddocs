# `param` Tag

The `param` tags allows adding documentation to a method or indexer parameter.

In the generated documentation, the text will be included in the
documentation section for the corresponding parameter.

## Supported members

| Member kind | Supported |
|-------------|-----------|
| Types       | No        |
| Fields      | No        |
| Events      | No        |
| Properties  | No        |
| Indexers    | Yes       |
| Methods     | Yes       |

For each member, there can be multiple  `param` tags (at most one tag for every
parameter).

## Usage

The required attribute `name` specifies which parameter the documentation
text applies to. The name must match the parameter name in the method
or indexer signature.

```xml
<param name="parameterName">Description of the parameter</param>  
```

## See also

- [`typeparam` Tag](./typeparam.md)
- [\<param\> (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/param)
