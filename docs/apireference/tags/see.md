# `see` Tag

The `<see />` tag allows adding an link to a text. In the generated
documentation, the link will be rendered as inline link in the documentation
text.

## Usage

While Visual Studio only supports referring to other code elements using the
`cref` attribute, mddocs supports linking to external resources (e.g. websites)
using the `href` attribute, too.

```xml
<summary>
Documentation text referring to another type, e.g. <see cref="System.String" />
</summary>
```

```xml
<summary>
Documentation text referring to a website, e.g. <see href="http://example.com" />
</summary>
```

In both cases, an optional text can be specified. If a text is specified,
it will be rendered as link text in the output instead of the type name
or url.

```xml
<summary>
Documentation text referring to another type, e.g. <see cref="System.String">Link text</see>
</summary>
```

```xml
<summary>
Documentation text referring to a website, e.g. <see href="http://example.com">Link text</see>
</summary>
```

When both a `cref` and a `href` attribute is present, the `cref` attribute´
takes precedence and the `href` attribute is ignored.

## See also

- [`seealso` Tag](./seealso.md)
- [\<see\> (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/see)
