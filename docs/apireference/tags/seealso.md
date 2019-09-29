# `seealso` Tag

The `<seealso/>` tag allows specifying a link to additional documentation
items. In the generated documentation, the links will be included in a
"See also" section at the end of the page.

## Supported members

| Member kind | Supported |
|-------------|-----------|
| Types       | Yes       |
| Fields      | Yes       |
| Events      | Yes       |
| Properties  | Yes       |
| Indexers    | Yes       |
| Methods     | Yes       |

For each member, multiple `seealso` tags can be added.

## Usage

While Visual Studio only supports referring to other code elements using the
`cref` attribute, mddocs supports linking to external resources (e.g. websites)
using the `href` attribute, too.

```xml
<seealso cref="System.String" />
```

```xml
<seealso href="http://example.com" />
```

In both cases, an optional text can be specified. If a text is specified,
it will be rendered as link text in the output instead of the type name
or url.

```xml
<seealso cref="System.String">Link text</seealso>
```

```xml
<seealso href="http://example.com">Link text</seealso>
```

When both a `cref` and a `href` attribute is present, the `cref` attribute´
takes precedence and the `href` attribute is ignored.

## See also

- [`see` Tag](./see.md)
- [\<seealso\> (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/seealso)
