# `typeparamref` Tag

The `typeparamref` allows referencing a type parameter of a generic type or
method inside a text block.

In the generated documentation, the tag will be rendered as link to the
documentation section for the corresponding type parameter.

## Usage

The required attribute `name` specifies the name of the parameter being
referenced.

```xml
<typeparamref name="T" />  
```

## See also

- [`paramref` Tag](./tags/paramref.md)
- [\<typeparamref\> (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/typeparamref)
