# DemoPropertyFlags Enum

**Namespace:** DemoProject

**Assembly:** DemoProject

An example of a "flag" enum

```csharp
[Flags]
public enum DemoPropertyFlags
{
    Flag1 = 0x1,
    Flag2 = 0x2,
    Flag3 = 0x4
}
```

**Inheritance:** Object → ValueType → Enum → DemoPropertyFlags

**Attributes:** FlagsAttribute

## Remarks

DemoPropertyFlags serves two purposes:

On the one hand it showcases the generated documentation for "flag" enums. For flag enums, the numeric values for the possible enum values are represented as hexadecimal numbers in contrast to decimal numbers for other enum types.

Furthermore, this enum is used in [DemoPropertyAnnotationAttribute](../DemoPropertyAnnotationAttribute/Type.md) and demonstrates             how custom attributes are included in the documentation for properties (see [Property2](../DemoClass/Properties/Property2.md))

## Fields

| Name  | Description              |
| ----- | ------------------------ |
| Flag1 | Example of an enum value |
| Flag2 | Example of an enum value |
| Flag3 | Example of an enum value |

## See Also

- [DemoPropertyAnnotationAttribute](../DemoPropertyAnnotationAttribute/Type.md)
- [Property2](../DemoClass/Properties/Property2.md)