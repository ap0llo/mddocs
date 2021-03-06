﻿<!--  
  <auto-generated>   
    The contents of this file were generated by a tool.  
    Changes to this file may be list if the file is regenerated  
  </auto-generated>   
-->

# ListExample Class

**Namespace:** [DemoProject](../index.md)  
**Assembly:** DemoProject

Example class to showcase the different supported list formats.

```csharp
public class ListExample
```

**Inheritance:** object → ListExample

## Remarks

To create a list, use the `list` tag. Using this tag, bulleted lists, numbered lists and two\-column tables can be created.

To create a bulleted list, use the following syntax:

```xml
<list type="bullet">
    <item>
        <description>Item 1</description>
    </item>
    <item>
        <description>Item 2</description>
    </item>
</list>
```

which is rendered as like this

- Item 1
- Item 2

By changing the type of the list to "number"

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

a numbered list will be rendered:

1. Item 1
2. Item 2

Both numbered and bulleted lists also support items that specify both a `description` and a `term`:

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

In this case, the value of `term` will be rendered as bold prefix for the description:

1. **Term 1:** 

   Item 1
2. **Term 2:** 

   Item 2

By changing the type of the list to `table`, the list will be rendered as two\-column table. To specify the header row of the table , use `listheader`

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

In this case, the value of `term` will be rendered as bold prefix for the description:

| Term            | Description     |
| --------------- | --------------- |
| Row 1, Column 1 | Row 1, Column 2 |
| Row 2, Column 1 | Row 2, Column 2 |

## Constructors

| Name                                   | Description |
| -------------------------------------- | ----------- |
| [ListExample()](constructors/index.md) |             |

___

*Documentation generated by [MdDocs](https://github.com/ap0llo/mddocs)*
