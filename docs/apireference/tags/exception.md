# `exception` Tag

The `exception` tag allows documenting exceptions that might be thrown
when calling a member.

## Supported members

| Member kind | Supported |
|-------------|-----------|
| Types       | No        |
| Fields      | No        |
| Events      | No        |
| Properties  | Yes       |
| Indexers    | No        |
| Methods     | Yes       |

For each member, multiple `exception` tags can be added.

## Usage

The required attribute `cref` specifies the type of exception that might be
thrown.

```xml
<exception cref="MyExceptionType" />
```

Optionally, documentation text can be added to describe under which condition
a exception of this type is thrown.

```xml
<exception cref="MyExceptionType">Description of the conditions a exception of this type is thrown</exception>  
```

## See also

- [\<exception\> (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/exception)
