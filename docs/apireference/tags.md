# API Reference: Supported documentation tags

## Overview

This document describes the documentation tags supported for generated
(C#) API referenced.

mddocs differentiates between two kinds of tags:

- "Top-level" tags used for annotating members. Each top-level tag contains
  text that may be annotated with inline tags.
  Optionally, some top-level tags may support or require additional attributes.
- Inline tags used for specifying the text within top-level tags.

## Top-level tags

- [summary](./tags/summary.md)
- [remarks](./tags/remarks.md)
- [example](./tags/example.md)
- [param](./tags/param.md)
- [typeparam](./tags/typeparam.md)
- [seealso](./tags/seealso.md)
- [exception](./tags/exception.md)
- [value](./tags/value.md)
- [returns](./tags/returns.md)

## Inline tags

- para
- paramref
- typeparamref
- code
- c
- [see](./tags/see.md)
- list

## See Also

- [Recommended Tags for Documentation Comments (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/recommended-tags-for-documentation-comments)
