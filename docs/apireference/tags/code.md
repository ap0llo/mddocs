# `code` Tag

The `code` tag allows adding blocks of code to text blocks.

In the generated Markdown file, the content will be rendered as fenced code block.

## Usage

The `code` tag can be used in every text block, e.g. in an `example` tag:

```xml
<example>
The example can be used to provide a usage example, e.g. how
to create a new instance of a type:
<code>
    var myInstance = new MyType();
    myInstance.Method1();
</code>
</example>
```

You can also specify the language of the code sample using the `language`
attribute:

```xml
<code language="csharp">
    var myInstance = new MyType();
    myInstance.Method1();
</code>
```

If a `language` attribute is present, the its value will be included as
"info string" for the code block in the generated markdown file. This info
string can be used by Markdown renderers to add syntax highlighting for the code
sample.

Alternatively, the language can also be specified using the `lang` attribute.
If both attributes are present, the `language` attribute takes precedence and
the value of `lang` is ignored.

## See also

- [`c` Tag](./c.md)
- [\<code\> (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/code)
- [Sandcastle XML Comments Guide - code](http://ewsoftware.github.io/XMLCommentsGuide/html/1abd1992-e3d0-45b4-b43d-91fcfc5e5574.htm)
- [Markdown Fenced code blocks](https://spec.commonmark.org/0.29/#fenced-code-blocks)
