# `list` Tag

Using the `list` tag, lists can be added to documentation text.

In the generated markdown, lists are rendered as either bulleted list, numbered
list or table.

## Bulleted and numbered list

The `list` tag can be used in every text block.

A list can contain one or more list items, each of them specified as a
`description` tag wrapped in an `item` element. The type of list (bulleted or
numbered) can be specified using the `type` attribute.

To create a bulleted list, for example in a `remarks` section, use:

```xml
<remarks>
    <list type="bullet">
        <item>
            <description>Item 1</description>
        </item>
        <item>
            <description>Item 2</description>
        </item>
    </list>
</remarks>
```

This will be converted to this Markdown list:

- Item 1
- Item 2

To create a numbered list, change the type to `number`:

```xml
<list type="number">
    <item>
        <description>Item 1</description>
    </item>
    <item>
        <description>Item 2</description>
    </item>
</list>
```

The example above will produce the following Markdown list:

1. Item 1
2. Item 2

**Note:** If no list type is specified, the list will be rendered as bulleted
list.

---

## Definition lists

For both numbered and bulleted lists, a `term` element can be added to each
item. This is useful for defining a list of terms, hence "definition list".

```xml
<list type="number">
    <item>
        <term>Term 1</term>
        <description>Item 1</description>
    </item>
    <item>
        <term>Term 2</term>
        <description>Item 2</description>
    </item>
</list>
```

The example above will be converted to the following Markdown list:

1. **Term 1:**

   Item 1
2. **Term 2:**

   Item 2

---

## Tables

Definition lists can also be rendered as (two-column) tables. By changing the
list type to `table`.

A header row can be added to the table by specifying a  `listheader` element:

```xml
<list type="table">
    <listheader>
        <term>Term</term>
        <description>Description</description>
    </listheader>
    <item>
        <term>Row 1, Column 1</term>
        <description>Row 1, Column 2</description>
    </item>
    <item>
        <term>Row 2, Column 1</term>
        <description>Row 2, Column 2</description>
    </item>
</list>
```

This list will be converted to the following Markdown table:

| Term            | Description     |
| --------------- | --------------- |
| Row 1, Column 1 | Row 1, Column 2 |
| Row 2, Column 1 | Row 2, Column 2 |

---

## See also

- [\<list\> (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/list)
