# `paramref` Tag

The `paramref` allows referencing a method's or indexer's parameter within
a text block.

In the generated documentation, the tag will be rendered as link to the
documentation section for the corresponding parameter.

## Usage

The required attribute `name` specifies the name of the parameter being
referenced.

```xml
<paramref name="parameterName" />  
```

## See also

- [`typeparamref` Tag](./tags/typeparamref.md)
- [\<paramref\> (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/paramref)
